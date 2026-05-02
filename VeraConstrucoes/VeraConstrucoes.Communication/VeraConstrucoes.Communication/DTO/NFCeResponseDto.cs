namespace VeraConstrucoes.Communication.DTO
{
    public class NFCeResponseDto
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string ChaveAcesso { get; set; }
        public string Numero { get; set; }
        public string Serie { get; set; }
        public DateTime DataEmissao { get; set; }
        public string Protocolo { get; set; }
        public string XML { get; set; }
        public string DanfeUrl { get; set; }
        public string QrCode { get; set; }
        public decimal ValorTotal { get; set; }
        public List<string> Erros { get; set; } = new List<string>();
    }
}
