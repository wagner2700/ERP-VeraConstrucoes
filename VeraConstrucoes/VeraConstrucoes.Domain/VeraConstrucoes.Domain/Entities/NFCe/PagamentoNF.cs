using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeraConstrucoes.Domain.Entities.NFCe
{
    public class PagamentoNF
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("NFCe")]
        public int NFCeId { get; set; }
        public NFC NFCe { get; set; }
        [MaxLength(20)]
        public string Metodo { get; set; } = string.Empty;             // dinheiro, credito, debito, pix

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }                  // Valor pago
        public int   Parcelas { get; set; }                  // Número de parcelas (crédito)

        [Column(TypeName = "decimal(18,2)")]
        public decimal Troco { get; set; }                 // Troco (dinheiro)
        [MaxLength(20)]
        public string Bandeira { get; set; } = string.Empty;
    }
}
