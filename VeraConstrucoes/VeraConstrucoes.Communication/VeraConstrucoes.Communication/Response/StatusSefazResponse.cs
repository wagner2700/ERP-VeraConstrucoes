namespace VeraConstrucoes.Communication.Response
{
    public class StatusSefazResponse
    {
        public bool Success { get; set; }
        public SefazStatusResult? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
