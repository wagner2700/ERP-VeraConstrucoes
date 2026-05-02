using Microsoft.EntityFrameworkCore;
using NFCVeraConstrucoes.Models.NFCe;
using VeraConstrucoes.Domain.Entities.Configuracoes;
using VeraConstrucoes.Domain.Entities.NCM;
using VeraConstrucoes.Domain.Entities.NFCe;
using VeraConstrucoes.Domain.Entities.Produtos;
using static NFCVeraConstrucoes.Models.NFCe.FormaPagamentoNFCe;

namespace VeraConstrucoes.Infrastructure.Data
{
    public class VeraConstrucoesContext : DbContext
    {



        public VeraConstrucoesContext(DbContextOptions<VeraConstrucoesContext> options)
          : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<NFCePropriedades> NFCePropriedades { get; set; }
        public DbSet<PagamentoNF> PagamentoNFCes { get; set; }
        public DbSet<NFC> NFCes { get; set; }
        public DbSet<NCM> NCM { get; set; }
        public DbSet<ConfiguracaoNFCe> ConfiguracoesNFCe { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            //Relacionamento 1:1 entre NFC e NFCePropriedades
            //modelBuilder.Entity<NFC>()
            //    .HasOne(n => n.Complemento)
            //    .WithOne(p => p.NFCe)
            //    .HasForeignKey<NFCePropriedades>(p => p.NFCeId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //Relacionamento 1:N entre NFC e ProdutoNF
            //modelBuilder.Entity<ProdutoNF>()
            //    .HasOne(p => p.NFCe)
            //    .WithMany(n => n.Produtos)
            //    .HasForeignKey(p => p.NFCeId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //Relacionamento 1:N entre NFC e PagamentoNF
            //modelBuilder.Entity<PagamentoNF>()
            //    .HasOne(p => p.NFCe)
            //    .WithMany(n => n.Pagamentos)
            //    .HasForeignKey(p => p.NFCeId)
            //    .OnDelete(DeleteBehavior.Cascade);

        }



    }
}
