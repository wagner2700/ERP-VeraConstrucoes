using VeraConstrucoes.Communication.Request;
using VeraConstrucoes.Domain.Entities.Produtos;

namespace VeraConstrucoes.Application.UseCases.Products.Interfaces
{
    public interface IWriteProductUseCase
    {
        Task UpdateProduto(RequestUpdateProductJson produto);   
    }
}
