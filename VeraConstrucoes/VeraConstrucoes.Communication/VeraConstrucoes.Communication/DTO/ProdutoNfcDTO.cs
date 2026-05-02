namespace VeraConstrucoes.Communication.DTO
{
    public class ProdutoNfcDTO
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public string Ncm { get; set; } = string.Empty;
        public int Cfop { get; set; } 
        public string Unidade { get; set; } = string.Empty;
        public string? Cest { get; set; }
    }
}
