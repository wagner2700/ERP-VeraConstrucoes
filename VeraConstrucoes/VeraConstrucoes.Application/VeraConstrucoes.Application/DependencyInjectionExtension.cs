using DFe.Utils;
using Microsoft.Extensions.DependencyInjection;
using NFCVeraConstrucoes.Services;
using VeraConstrucoes.Application.AutoMapper;
using VeraConstrucoes.Application.Services;
using VeraConstrucoes.Application.UseCases.Configuracoes;
using VeraConstrucoes.Application.UseCases.Configuracoes.Interface;
using VeraConstrucoes.Application.UseCases.NFC;
using VeraConstrucoes.Application.UseCases.NFC.Interface;
using VeraConstrucoes.Application.UseCases.Products;
using VeraConstrucoes.Application.UseCases.Products.Interfaces;
using VeraConstrucoes.Infrastructure.Repositories;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Application
{
    public static class DependencyInjectionExtension
    {

        public static void AddApplication(this IServiceCollection services)
        {
            AddUseCases(services);
            AddAutoMapper(services);
        }


        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapping));
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterProductUseCase, RegisterProductUseCase>();
            services.AddScoped<IGetAllProductUseCase, GetAllProductUseCase>();
            services.AddScoped<ISefazServiceUseCase, SefazServiceUseCase>();
            services.AddScoped<IEmitNFCUseCase, EmitNFCUseCase>();
            services.AddScoped<ISaveNFCUseCase, SaveNFCUseCase>();
            services.AddScoped<IReadOnlyNotaFiscalUseCase, ReadOnlyNotaFiscalUseCase>();
            services.AddScoped<IWriteProductUseCase, WriteProductUseCase>();
            services.AddScoped<IEmitNFCUseCase, EmitNFCUseCase>(); 
            services.AddScoped<INFCeMapper, NFCeMapper>();
            services.AddScoped<IConfiguracaoRepository, ConfiguracaoRepository>();
            services.AddScoped<IGerenciarConfiguracaoUseCase, GerenciarConfiguracaoUseCase>();

            services.AddScoped<CertificadoServices>();
            services.AddScoped<ConfiguracaoCertificado>();
            
            services.AddScoped<INFCService ,NFCService>();
            




        }
    }
}
