using Microsoft.EntityFrameworkCore;
using VeraConstrucoes.Domain.Entities.Produtos;
using VeraConstrucoes.Infrastructure.Data;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Infrastructure.Repositories
{
    public class ProductRepository : IProductWriteOnlyRepository , IProductReadOnlyRepository
    {
        private readonly VeraConstrucoesContext _context;

        public ProductRepository(VeraConstrucoesContext context)  => _context = context;
        public async Task Add(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Produto>> BuscarPorCodigoOuDescricao(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return new List<Produto>();

            // Tenta converter para int para buscar por código (id)
            if (int.TryParse(termo, out int codigo))
            {
                // Busca por código exato
                var produto = await _context.Produtos
                    .Include(p => p.ncm)
                    .FirstOrDefaultAsync(p => p.id == codigo);
                if (produto != null)
                    return new List<Produto> { produto };
            }

            // Busca por descrição (case insensitive, contém)
            return await _context.Produtos
                .Include(p => p.ncm)
                .Where(p => EF.Functions.Like(p.descricao, $"%{termo}%"))
                .ToListAsync();
        }

        public async Task<List<Produto>> GetAll()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        public async Task<(List<Produto> items, int total)> GetAllPaged(int page, int pageSize)
        {
            var query = _context.Produtos.AsNoTracking();
            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task Update(Produto produto)
        {
            var item = await _context.Produtos.FirstOrDefaultAsync(id => id.id == produto.id);
            if(item != null)
            {
                item.Ncm = produto.Ncm;
                item.descricao = produto.descricao;
                item.valorUnitario = produto.valorUnitario;
                item.estoque = produto.estoque;
            }
            
            await _context.SaveChangesAsync();
        }
    }
}
