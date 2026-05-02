using Microsoft.EntityFrameworkCore;
using VeraConstrucoes.Domain.Entities.Configuracoes;
using VeraConstrucoes.Infrastructure.Data;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Infrastructure.Repositories
{
    public class ConfiguracaoRepository : IConfiguracaoRepository
    {
        private readonly VeraConstrucoesContext _context;

        public ConfiguracaoRepository(VeraConstrucoesContext context)
        {
            _context = context;
        }

        public async Task<ConfiguracaoNFCe?> ObterConfiguracaoAsync()
        {
            // Assume que existe apenas um registro com Id = 1
            return await _context.ConfiguracoesNFCe.FindAsync(1);
        }

        public async Task AtualizarConfiguracaoAsync(ConfiguracaoNFCe config)
        {
            _context.ConfiguracoesNFCe.Update(config);
            await _context.SaveChangesAsync();
        }

        public async Task InserirConfiguracaoAsync(ConfiguracaoNFCe config)
        {
            config.Id = 1; // Garante o Id fixo
            await _context.ConfiguracoesNFCe.AddAsync(config);
            await _context.SaveChangesAsync();
        }

        public async Task<int> ObterProximoNumeroNota()
        {
            int numeroNota = 1;
            var config = await _context.ConfiguracoesNFCe.FirstOrDefaultAsync(id => id.Id == 1);
            if(config != null)
            {
                numeroNota = config.UltimoNumeroNota + 1;
                config.UltimoNumeroNota = numeroNota;
                await _context.SaveChangesAsync();
                return numeroNota;
            }
            return numeroNota;
        }


        public async Task<int> ObterProximoLote()
        {
            var config = await _context.ConfiguracoesNFCe.FirstOrDefaultAsync(id => id.Id == 1);
            int NumeroLote = 1;

            if(config != null)
            {
                NumeroLote = config.NumeroLote + 1;

                config.NumeroLote = NumeroLote;
                await _context.SaveChangesAsync();
            }
            return NumeroLote;
        }
        
    }
}
