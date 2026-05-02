namespace VeraConstrucoes.Communication.DTO
{
    public class NFCeResumoDTO
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Serie { get; set; }
        public DateTime DataEmissao { get; set; }
        public decimal ValorTotal { get; set; }
        public bool StatusProcessamento { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteDocumento { get; set; } = string.Empty;
    }
}
