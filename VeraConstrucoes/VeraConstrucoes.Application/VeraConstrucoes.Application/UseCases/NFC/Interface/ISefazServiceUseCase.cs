using NFe.Classes.Servicos.Status;
using VeraConstrucoes.Communication.Response;

namespace VeraConstrucoes.Application.UseCases.NFC.Interface
{
    public interface ISefazServiceUseCase
    {
        Task<retConsStatServ> CheckStatusAsync();
        bool TestarConexaoSefaz();
    }
}
