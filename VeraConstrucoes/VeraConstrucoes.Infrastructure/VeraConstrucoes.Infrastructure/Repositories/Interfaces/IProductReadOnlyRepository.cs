using VeraConstrucoes.Domain.Entities.Produtos;

namespace VeraConstrucoes.Infrastructure.Repositories.Interfaces
{
    public interface IProductReadOnlyRepository
    {


        Task<List<Produto>> GetAll();

        Task<IEnumerable<Produto>> BuscarPorCodigoOuDescricao(string termo);

        Task<(List<Produto> items, int total)> GetAllPaged(int page, int pageSize);
    }
}
