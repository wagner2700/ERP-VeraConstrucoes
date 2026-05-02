using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VeraConstrucoes.Domain.Entities.NCM;

namespace VeraConstrucoes.Domain.Entities.Produtos
{
    public class Produto
    {

        public int id { get; set; }
        [MaxLength(255)]
        public string descricao {  get; set; } = string.Empty;
        [Column(TypeName = "decimal(10,2)")]
        public decimal valorUnitario { get; set; }
        public int estoque {  get; set; }

        public string? Ncm { get; set; } = string.Empty;

        [ForeignKey("Ncm")]
        public NCM.NCM? ncm { get; set; }
    }
}
