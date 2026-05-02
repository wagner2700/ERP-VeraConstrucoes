using NFCVeraConstrucoes.Models;
using NFCVeraConstrucoes.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5137");

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<DashboardDataService>();

var app = builder.Build();

app.UseCors("PermitirAngular");

app.MapGet("/", () => Results.Ok("API NFC-e Rodando!"));

app.MapGet("/api/dashboard/summary", (DashboardDataService dashboardDataService) =>
{
    var summary = dashboardDataService.GetSummary();
    return Results.Ok(summary);
});

app.Run();
