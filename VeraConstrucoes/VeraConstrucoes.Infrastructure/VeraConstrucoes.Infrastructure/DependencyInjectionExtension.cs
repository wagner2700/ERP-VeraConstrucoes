using Microsoft.Extensions.DependencyInjection;
using VeraConstrucoes.Infrastructure.Repositories;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Infrastructure
{
    public static class DependencyInjectionExtension
    {

        public static void AddInfrastructure(this IServiceCollection services)
        {
            AddRepositories(services);
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IProductWriteOnlyRepository, ProductRepository>();
            services.AddScoped<IProductReadOnlyRepository, ProductRepository>();
            services.AddScoped<INFCRepository, NFCRepository>();
            services.AddScoped<INFCRepository, NFCRepository>();
            services.AddScoped<INCMRepository, NCMRepository> ();



        }
    }
}
