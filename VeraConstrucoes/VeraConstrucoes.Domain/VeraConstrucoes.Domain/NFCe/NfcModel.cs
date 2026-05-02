
using VeraConstrucoes.Domain.Entities.NFCe;

namespace VeraConstrucoes.Domain.NFCe;

public class NfcModel
{
    public int Id { get; set; }
    
    public int? Numero { get; set; }
    public int? Serie { get; set; }
    public DateTime? DataEmissao { get; set; }
    
    public decimal? ValorTotal { get; set; }
    public bool? Sucesso { get; set; }
    
    
    public string? ClienteDocumento { get; set; } = string.Empty;     // CPF/CNPJ
    
    public string? ClienteNome { get; set; } = string.Empty;         // Nome/Razão Social
    
    public string? ClienteEmail { get; set; } = string.Empty;        // Email
    
    public string? ClienteTelefone { get; set; } = string.Empty;     // Telefone


    // Relacionamento 1:1 com complemento
    public NFCePropriedades? Complemento { get; set; }
    // Relacionamentos
    public ICollection<ProdutoNF> Produtos { get; set; } = new List<ProdutoNF>();
    public ICollection<PagamentoNF> Pagamentos { get; set; } = new List<PagamentoNF>();
}
