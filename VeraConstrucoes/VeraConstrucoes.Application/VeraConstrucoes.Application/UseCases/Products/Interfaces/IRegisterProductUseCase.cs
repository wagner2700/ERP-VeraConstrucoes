using VeraConstrucoes.Communication.Request;
using VeraConstrucoes.Communication.Response;

namespace VeraConstrucoes.Application.UseCases.Products.Interfaces
{
    public interface IRegisterProductUseCase
    {
        Task<ResponseRegisterProduct> Execute(RequestRegisterProduct request);
    }
}
