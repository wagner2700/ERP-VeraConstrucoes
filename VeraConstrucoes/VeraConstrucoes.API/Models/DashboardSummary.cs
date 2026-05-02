namespace VeraConstrucoes.API.Models
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
}
