using VeraConstrucoes.Communication.DTO;
using VeraConstrucoes.Domain.NFCe;

namespace VeraConstrucoes.Application.UseCases.NFC.Interface
{
    public interface IReadOnlyNotaFiscalUseCase
    {

        Task<NfcDetalhesDTO> LerDetalhesNotaFiscal(int id);

        Task<List<NFCeResumoDTO>> ListarNotasFiscais();
    }
}
