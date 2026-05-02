using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeraConstrucoes.Domain.Entities.NFCe
{
    public class NFCePropriedades
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("NFCe")]
        public int NFCeId { get; set; }
        public NFC? NFCe { get; set; }

        [MaxLength(500)]
        public string Observacoes { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Desconto { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Acrescimo { get; set; }
        [MaxLength(500)]
        public string XmlPath { get; set; } = string.Empty;
        [MaxLength(500)]
        public string PdfPath { get; set; } = string.Empty;
        [MaxLength(500)]
        public string QrCodeUrl { get; set; } = string.Empty;
        [MaxLength(500)]
        public string MensagemErro { get; set; } = string.Empty;
        [MaxLength(50)]
        public string ChaveAcesso { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Protocolo { get; set; } = string.Empty;
    }
}
