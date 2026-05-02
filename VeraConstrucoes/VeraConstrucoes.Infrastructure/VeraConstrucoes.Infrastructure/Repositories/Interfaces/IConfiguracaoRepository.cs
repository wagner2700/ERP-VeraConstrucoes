using VeraConstrucoes.Domain.Entities.Configuracoes;

namespace VeraConstrucoes.Infrastructure.Repositories.Interfaces
{
    public interface IConfiguracaoRepository
    {
        Task<ConfiguracaoNFCe?> ObterConfiguracaoAsync();
        Task AtualizarConfiguracaoAsync(ConfiguracaoNFCe config);
        Task InserirConfiguracaoAsync(ConfiguracaoNFCe config);
        Task<int> ObterProximoNumeroNota();
        Task<int> ObterProximoLote();
    }
}
