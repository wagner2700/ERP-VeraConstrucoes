using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeraConstrucoes.Domain.Entities.NFCe
{
    public class ProdutoNF
    {
        [Key]
        public int? Id { get; set; }
        [ForeignKey("NFCe")]
        public int NFCeId { get; set; }
        public NFC NFCe { get; set; }

        [MaxLength(20)]
        public string Codigo { get; set; } = string.Empty;            // Código do produto
        [MaxLength(200)]
        public string Descricao { get; set; } = string.Empty;            // Descrição

        [Column(TypeName = "decimal(18,3)")]
        public decimal Quantidade { get; set; }           // Quantidade
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorUnitario { get; set; }         // Valor unitário
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal => Quantidade *ValorUnitario;
        [MaxLength(8)]
        public string Ncm { get; set; } = string.Empty;              // NCM (8 dígitos)
        [MaxLength(4)]
        public int Cfop { get; set; }                 // CFOP
        [MaxLength(6)]
        public string Unidade { get; set; } = string.Empty;             // Unidade (ex: UN)
        [MaxLength(7)]
        public string Cest { get; set; } = string.Empty;              // CEST (opcional)
    }
}
