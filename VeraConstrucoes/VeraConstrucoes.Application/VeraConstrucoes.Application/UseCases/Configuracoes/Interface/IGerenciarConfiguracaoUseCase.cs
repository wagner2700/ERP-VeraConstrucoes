using VeraConstrucoes.Domain.Entities.Configuracoes;

namespace VeraConstrucoes.Application.UseCases.Configuracoes.Interface
{
    public interface IGerenciarConfiguracaoUseCase
    {
        Task<ConfiguracaoNFCe?> ObterConfiguracao();
        Task<ConfiguracaoNFCe> SalvarConfiguracao(ConfiguracaoNFCe config);

        
    }
}
