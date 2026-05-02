namespace VeraConstrucoes.Communication.Response
{
    public class ResponseRegisterProduct
    {
        public int id {  get; set; }
        public string descricao {  get; set; } = string.Empty;
        public decimal valorUnitario { get; set; } = 0;
        public int estoque { get; set; } = 0;
        public string Ncm {  get; set; } = string.Empty;
    }
}
