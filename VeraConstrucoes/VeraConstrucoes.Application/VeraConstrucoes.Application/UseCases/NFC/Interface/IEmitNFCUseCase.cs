using NFCVeraConstrucoes.Models;
using NFe.Servicos.Retorno;
using VeraConstrucoes.Communication.DTO;
using VeraConstrucoes.Communication.Request;

namespace VeraConstrucoes.Application.UseCases.NFC.Interface
{
    public interface IEmitNFCUseCase
    {
        Task<ResultadoEmissao> Execute(NFCeRequestDto request);

        Task DownloadXml(string chave);
        Task<RetornoRecepcaoEvento> CancelamentoNFCe(CancelamenoNotaFiscalRequest cancelamenoNota);
    }
}
