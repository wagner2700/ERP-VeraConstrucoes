namespace NFCVeraConstrucoes.Models.Produtos
{
    public class ProdutoModel
    {
        public string codigoProduto { get; set; } = string.Empty;
        public string descrição {  get; set; } = string.Empty;
        public decimal valorUnitario { get; set; }
    }
}
