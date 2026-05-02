using VeraConstrucoes.Communication.Request;
using VeraConstrucoes.Domain.Entities.Produtos;

namespace VeraConstrucoes.Infrastructure.Repositories.Interfaces
{
    public interface IProductWriteOnlyRepository
    {
        Task Add(Produto produto);
        Task Update(Produto produto);
    }
}
