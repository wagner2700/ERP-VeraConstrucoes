namespace VeraConstrucoes.Communication.Response
{
    public class SefazStatusResult
    {
        public string CodigoStatus { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public DateTime? DataResposta { get; set; }
        public int TempoMedioResposta { get; set; }
        public string VersaoAplicacao { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
        public DateTime DataConsulta { get; set; } = DateTime.UtcNow;
    }
}
