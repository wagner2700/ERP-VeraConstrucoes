using NFCVeraConstrucoes.Models;

namespace VeraConstrucoes.Application.Services
{
    public interface INFCService
    {
        Task<ResultadoEmissao> EmitirNFCe(NFe.Classes.NFe nfce);
    }
}
