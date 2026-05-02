using VeraConstrucoes.Domain.Entities.NCM;

namespace VeraConstrucoes.Infrastructure.Repositories.Interfaces
{
    public interface INCMRepository
    {
        Task<NCM?> GetDadosNcm(string ncm);
        Task<List<NCM>> ListarTodosAsync();
    }
}
