using VeraConstrucoes.Communication.DTO;
using VeraConstrucoes.Domain.Entities.NFCe;

namespace VeraConstrucoes.Infrastructure.Repositories.Interfaces
{
    public interface INFCRepository
    {

        Task<NFC> AddNFC(NFC nfc);

        Task<NfcDetalhesDTO?> ObterDetalhesCompletoAsync(int id);

        Task<List<NFCeResumoDTO>> ObterNotasFiscais();
        Task<NFC?> GetByChaveAcessoAsync(string chaveAcesso);
        Task<NFC> AddOrUpdateNFC(NFC nfc);
        Task UpdateSituacaoNotaFiscal(int NumeroNf, string situacaoNota);
    }
}
