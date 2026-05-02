using DFe.Utils;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Data.Common;
using System.Reflection;
using VeraConstrucoes.Application;
using VeraConstrucoes.Application.UseCases.NFC;
using VeraConstrucoes.Application.UseCases.NFC.Interface;
using VeraConstrucoes.API.Services;
using VeraConstrucoes.Infrastructure;
using VeraConstrucoes.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
var enableElectron = ShouldEnableElectron(builder.Configuration);
var connectionString = ResolveMySqlConnectionString(builder.Configuration);

ConfigureContainerBinding(builder);

// Add services to the container.


builder.Services.AddDbContext<VeraConstrucoesContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));


builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddSingleton<DashboardDataService>();

// Registrar o use case
builder.Services.AddScoped<IEmitNFCUseCase, EmitNFCUseCase>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo",
        policy => policy.AllowAnyOrigin() // Em produção você restringe ao localhost:4200
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
// Adicione esta configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VeraConstrucoes API",
        Version = "v1",
        Description = "API para emissão de NFC-e"
    });

    // Resolver conflitos de nomes usando o nome completo do tipo
    c.CustomSchemaIds(type => type.FullName?.Replace("+", "."));

    // Se persistir o erro, tente esta alternativa mais agressiva:
    // c.CustomSchemaIds(type => type.ToString());

    // Configurações adicionais recomendadas
    c.UseAllOfForInheritance();
    c.UseOneOfForPolymorphism();

    // Adicionar suporte para comentários XML (opcional, mas recomendado)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

if (enableElectron)
{
    builder.WebHost.UseElectron(args);
}

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Inicia a janela do Electron quando o app estiver pronto
if (enableElectron && HybridSupport.IsElectronActive)
{
    ElectronBootstrap();
}

app.UseCors("PermitirTudo");



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/dashboard/summary", (DashboardDataService dashboardDataService) =>
{
    var summary = dashboardDataService.GetSummary();
    return Results.Ok(summary);
});

app.MapControllers();

app.Run();


async void ElectronBootstrap()
{
    var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
    {
        Width = 1200,
        Height = 800,
        Show = false, // Começa invisível, mostra quando carregar
        AutoHideMenuBar = true,
        Title = "Vera Construções - NFC-e"
    });

    browserWindow.OnReadyToShow += () => browserWindow.Show();
    browserWindow.OnClosed += () => Electron.App.Quit();
}

static void ConfigureContainerBinding(WebApplicationBuilder builder)
{
    var port = builder.Configuration["PORT"];
    var urls = builder.Configuration["ASPNETCORE_URLS"];

    if (!string.IsNullOrWhiteSpace(port) && string.IsNullOrWhiteSpace(urls))
    {
        builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
    }
}

static bool ShouldEnableElectron(ConfigurationManager configuration)
{
    var configuredValue = configuration["ENABLE_ELECTRON"];

    if (bool.TryParse(configuredValue, out var enableElectron))
    {
        return enableElectron;
    }

    return OperatingSystem.IsWindows();
}

static string ResolveMySqlConnectionString(ConfigurationManager configuration)
{
    var railwayConnectionString = BuildRailwayMySqlConnectionString(configuration);

    if (!string.IsNullOrWhiteSpace(railwayConnectionString))
    {
        return railwayConnectionString;
    }

    var configuredConnectionString = configuration.GetConnectionString("DefaultConnection");

    if (!string.IsNullOrWhiteSpace(configuredConnectionString))
    {
        return configuredConnectionString;
    }

    throw new InvalidOperationException(
        "Nenhuma string de conexao MySQL foi encontrada. Defina ConnectionStrings__DefaultConnection ou as variaveis MYSQLHOST, MYSQLPORT, MYSQLDATABASE, MYSQLUSER e MYSQLPASSWORD.");
}

static string? BuildRailwayMySqlConnectionString(ConfigurationManager configuration)
{
    var host = configuration["MYSQLHOST"];

    if (!string.IsNullOrWhiteSpace(host))
    {
        return BuildMySqlConnectionString(
            host,
            configuration["MYSQLPORT"],
            configuration["MYSQLDATABASE"],
            configuration["MYSQLUSER"],
            configuration["MYSQLPASSWORD"]);
    }

    var mysqlUrl = configuration["MYSQL_URL"];

    if (string.IsNullOrWhiteSpace(mysqlUrl))
    {
        return null;
    }

    if (!mysqlUrl.StartsWith("mysql://", StringComparison.OrdinalIgnoreCase))
    {
        return mysqlUrl;
    }

    var uri = new Uri(mysqlUrl);
    var userInfo = uri.UserInfo.Split(':', 2, StringSplitOptions.None);
    var user = userInfo.Length > 0 ? Uri.UnescapeDataString(userInfo[0]) : string.Empty;
    var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;
    var database = uri.AbsolutePath.Trim('/');

    return BuildMySqlConnectionString(
        uri.Host,
        uri.Port.ToString(),
        database,
        user,
        password);
}

static string BuildMySqlConnectionString(string server, string? port, string? database, string? user, string? password)
{
    if (string.IsNullOrWhiteSpace(server) ||
        string.IsNullOrWhiteSpace(database) ||
        string.IsNullOrWhiteSpace(user) ||
        string.IsNullOrWhiteSpace(password))
    {
        throw new InvalidOperationException(
            "As variaveis do MySQL estao incompletas. Verifique MYSQLHOST, MYSQLPORT, MYSQLDATABASE, MYSQLUSER e MYSQLPASSWORD.");
    }

    var builder = new DbConnectionStringBuilder
    {
        ["Server"] = server,
        ["Port"] = string.IsNullOrWhiteSpace(port) ? "3306" : port,
        ["Database"] = database,
        ["User Id"] = user,
        ["Password"] = password
    };

    return builder.ConnectionString;
}

