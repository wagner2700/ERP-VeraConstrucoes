namespace VeraConstrucoes.API.Models
{
    public class DashboardNota
    {
        public string Id { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Serie { get; set; } = string.Empty;
        public DateTime DataEmissao { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ClienteNome { get; set; }
    }
}
