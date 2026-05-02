namespace VeraConstrucoes.Communication.DTO
{
    public class NfcDetalhesDTO
    {
        // Dados da tabela nfces
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Serie { get; set; }
        public DateTime DataEmissao { get; set; }
        public decimal ValorTotal { get; set; }
        public string SituacaoNota { get; set; } = string.Empty;
        public bool StatusProcessamento { get; set; } 
        public string ClienteDocumento { get; set; } = string.Empty;
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteEmail { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;

        // Dados da tabela nfcepropriedades (relacionada)
        public string Observacoes { get; set; } = string.Empty;
        public decimal Desconto { get; set; }
        public decimal Acrescimo { get; set; }
        public string XmlPath { get; set; } = string.Empty;
        public string PdfPath { get; set; } = string.Empty;
        public string QrCodeUrl { get; set; } = string.Empty;
        public string MensagemErro { get; set; } = string.Empty;
        public string ChaveAcesso { get; set; } = string.Empty;
        public string Protocolo { get; set; } = string.Empty;

        // Listas relacionadas
        public List<ProdutoNfcDTO> Produtos { get; set; } = new();
        public List<PagamentoNfcDTO> Pagamentos { get; set; } = new();
    }
}
