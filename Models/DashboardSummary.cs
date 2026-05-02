namespace NFCVeraConstrucoes.Models
{
    public class DashboardSummary
    {
        public int TotalNotasFiscais { get; set; }
        public int NotasEmitidasHoje { get; set; }
        public int NotasCanceladasHoje { get; set; }
        public decimal ValorTotalNotasHoje { get; set; }
        public decimal ValorMedioNotas { get; set; }
        public List<DashboardNota> UltimasNotas { get; set; } = new();
    }

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
