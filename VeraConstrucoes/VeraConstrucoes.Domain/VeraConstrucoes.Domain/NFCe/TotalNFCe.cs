using NFe.Classes.Informacoes.Total;

namespace NFCVeraConstrucoes.Models.NFCe
{
    public class TotalNFCe
    {
        public decimal ValorTotalProdutos { get; set; }
        public decimal ValorBaseCalculoICMS { get; set; }
        public decimal ValorICMS { get; set; }
        public decimal ValorTotalNota { get; set; }
        public decimal ValorTotalTributos { get; set; }
        public decimal ValorICMSDesonerado { get; set; }

        public total ToZeusModel()
        {
            return new total
            {
                ICMSTot = new ICMSTot
                {
                    vBC = ValorBaseCalculoICMS,
                    vICMS = ValorICMS,
                    vProd = ValorTotalProdutos,
                    vNF = ValorTotalNota,
                    vTotTrib = ValorTotalTributos,
                    vICMSDeson = ValorICMSDesonerado,
                    vFCP = 0,
                    vFCPST = 0,
                    vFCPSTRet = 0,
                    vIPIDevol = 0,

                }
            };
        }
        public static TotalNFCe CalcularTotais(List<ProdutoNFCe> produtos)
        {
            var total = new TotalNFCe();

            foreach (var produto in produtos)
            {
                total.ValorTotalProdutos += produto.ValorTotal;
                total.ValorBaseCalculoICMS += produto.ValorBaseCalculo;
                total.ValorICMS += produto.ValorICMS;
                total.ValorTotalTributos += produto.ValorICMS;
            }
            total.ValorICMSDesonerado = 0;
            total.ValorTotalNota = total.ValorTotalProdutos;

            return total;
        }
    }
}
