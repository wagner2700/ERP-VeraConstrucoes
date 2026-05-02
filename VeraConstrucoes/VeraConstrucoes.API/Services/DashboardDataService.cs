using System.Globalization;
using System.Xml.Linq;
using VeraConstrucoes.API.Models;

namespace VeraConstrucoes.API.Services
{
    public class DashboardDataService
    {
        private readonly string _xmlDirectory;
        private static readonly XNamespace Ns = "http://www.portalfiscal.inf.br/nfe";

        public DashboardDataService()
        {
            _xmlDirectory = Path.Combine(Directory.GetCurrentDirectory(), "XMLs_Console");
            if (!Directory.Exists(_xmlDirectory))
            {
                Directory.CreateDirectory(_xmlDirectory);
            }
        }

        public DashboardSummary GetSummary()
        {
            var notas = Directory.EnumerateFiles(_xmlDirectory, "*.xml", SearchOption.TopDirectoryOnly)
                .Select(TryParseNotaFiscal)
                .Where(n => n != null)
                .Cast<DashboardNota>()
                .OrderByDescending(n => n.DataEmissao)
                .ToList();

            var hoje = DateTime.Now.Date;
            var notasHoje = notas.Where(n => n.DataEmissao.Date == hoje).ToList();
            var valorMedioNotas = notas.Any() ? Math.Round(notas.Average(n => n.ValorTotal), 2) : 0m;

            return new DashboardSummary
            {
                TotalNotasFiscais = notas.Count,
                NotasEmitidasHoje = notasHoje.Count,
                NotasCanceladasHoje = 0,
                ValorTotalNotasHoje = notasHoje.Sum(n => n.ValorTotal),
                ValorMedioNotas = valorMedioNotas,
                UltimasNotas = notas.Take(10).ToList()
            };
        }

        private DashboardNota? TryParseNotaFiscal(string filePath)
        {
            try
            {
                var document = XDocument.Load(filePath);
                var infNFe = document.Root?.Element(Ns + "infNFe");
                if (infNFe == null)
                {
                    return null;
                }

                var ide = infNFe.Element(Ns + "ide");
                var total = infNFe.Element(Ns + "total")?.Element(Ns + "ICMSTot");
                var dest = infNFe.Element(Ns + "dest");
                var emit = infNFe.Element(Ns + "emit");

                var dhEmiText = ide?.Element(Ns + "dhEmi")?.Value;
                var numeroText = ide?.Element(Ns + "nNF")?.Value ?? string.Empty;
                var serieText = ide?.Element(Ns + "serie")?.Value ?? string.Empty;
                var valorText = total?.Element(Ns + "vNF")?.Value ?? "0";
                var clienteNome = dest?.Element(Ns + "xNome")?.Value
                    ?? emit?.Element(Ns + "xNome")?.Value
                    ?? string.Empty;

                var dataEmissao = DateTime.TryParse(dhEmiText, null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var parsedDate)
                    ? parsedDate.ToLocalTime()
                    : File.GetLastWriteTime(filePath);

                var valorTotal = decimal.TryParse(valorText, NumberStyles.Any, CultureInfo.InvariantCulture, out var valor)
                    ? valor
                    : 0m;

                return new DashboardNota
                {
                    Id = infNFe.Attribute("Id")?.Value ?? Path.GetFileNameWithoutExtension(filePath),
                    Numero = numeroText,
                    Serie = serieText,
                    DataEmissao = dataEmissao,
                    ValorTotal = valorTotal,
                    Status = "emitida",
                    ClienteNome = clienteNome
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
