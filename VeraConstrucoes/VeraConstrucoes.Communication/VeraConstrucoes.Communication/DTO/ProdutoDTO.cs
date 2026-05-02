namespace VeraConstrucoes.Communication.DTO
{
    public class ProdutoDto
    {
        public int id { get; set; }
        public string Descricao { get; set; }
        public string NCM { get; set; }
        public int CFOP { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public string Unidade { get; set; } = "UN";
        public decimal? ICMS { get; set; } // Percentual
        public decimal? PIS { get; set; }
        public decimal? COFINS { get; set; }
        public decimal ValorTotal => Quantidade * ValorUnitario;


    }
}
