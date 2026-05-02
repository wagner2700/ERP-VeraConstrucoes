namespace VeraConstrucoes.Communication.Request
{
    public class RequestRegisterProduct
    {
        public string descricao {  get; set; } = string.Empty;
        public string unidadeMedida {  get; set; } = string.Empty;
        public decimal valorUnitario { get; set; }
        public int estoque { get; set; }
        public string? ncm { get; set; }
    }
}
