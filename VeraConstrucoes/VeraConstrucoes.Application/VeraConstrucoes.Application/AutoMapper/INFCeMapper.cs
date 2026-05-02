using NFCVeraConstrucoes;
using NFCVeraConstrucoes.Models;
using VeraConstrucoes.Communication.DTO;

namespace VeraConstrucoes.Application.AutoMapper
{
    public interface INFCeMapper
    {
        Task<NFe.Classes.NFe> ToNFeEntity(NFCeRequestDto dto);
    }
}