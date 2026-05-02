using NFCVeraConstrucoes.Models;
using VeraConstrucoes.Communication.DTO;
using VeraConstrucoes.Domain.Entities.NFCe;

namespace VeraConstrucoes.Application.UseCases.NFC.Interface
{
    public interface ISaveNFCUseCase
    {
        Task<ResultadoEmissao> Execute(NFCeRequestDto request, ResultadoEmissao resultado);
        Task<Domain.Entities.NFCe.NFC> GravarDadosIniciaisNotaFiscal(NFCeRequestDto request);
    }
}
