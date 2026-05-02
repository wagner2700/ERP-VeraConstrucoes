namespace VeraConstrucoes.Communication.DTO
{
    public class PagamentoNfcDTO
    {
        public string Metodo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public int Parcelas { get; set; }
        public decimal Troco { get; set; }
        public string Bandeira { get; set; } = string.Empty;
    }
}
