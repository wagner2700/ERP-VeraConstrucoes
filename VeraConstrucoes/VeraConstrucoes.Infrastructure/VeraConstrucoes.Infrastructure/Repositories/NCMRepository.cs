using Microsoft.EntityFrameworkCore;
using VeraConstrucoes.Domain.Entities.NCM;
using VeraConstrucoes.Infrastructure.Data;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Infrastructure.Repositories
{
    public class NCMRepository : INCMRepository
    {
        private readonly VeraConstrucoesContext _context;

        public NCMRepository(VeraConstrucoesContext context)
        {
            _context = context;
        }

        public async Task<NCM?> GetDadosNcm(string ncm)
        {
            return await _context.NCM.FindAsync(ncm);
        }

        public async Task<List<NCM>> ListarTodosAsync()
        {
            return await _context.NCM
                .AsNoTracking()
                .OrderBy(item => item.Ncm)
                .ToListAsync();
        }
    }
}
