using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeraConstrucoes.Domain.Entities.NFCe
{
    public class NFC
    {
        [Key]
        public int Id { get; set; }
        
        public int Numero { get; set; }
        public int Serie { get; set; }
        public DateTime DataEmissao { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }
        [MaxLength(1)]
        public string SituacaoNota { get; set; } = "L";// Lançada

        public bool StatusProcessamento { get; set; }
        
        [MaxLength(20)]
        public string ClienteDocumento { get; set; } = string.Empty;     // CPF/CNPJ
        [MaxLength(100)]
        public string ClienteNome { get; set; } = string.Empty;         // Nome/Razão Social
        [MaxLength(100)]
        [EmailAddress]
        public string ClienteEmail { get; set; } = string.Empty;        // Email
        [MaxLength(20)]
        public string ClienteTelefone { get; set; } = string.Empty;     // Telefone


        // Relacionamento 1:1 com complemento
        public NFCePropriedades Complemento { get; set; }
        // Relacionamentos
        public ICollection<ProdutoNF> Produtos { get; set; } = new List<ProdutoNF>();
        public ICollection<PagamentoNF> Pagamentos { get; set; } = new List<PagamentoNF>();
    }
}
