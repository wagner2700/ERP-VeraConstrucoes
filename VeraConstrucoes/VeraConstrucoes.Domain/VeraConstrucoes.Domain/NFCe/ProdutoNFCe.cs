using DFe.Classes.Flags;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace NFCVeraConstrucoes.Models.NFCe
{
    public class ProdutoNFCe
    {
        public int NumeroItem { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string NCM { get; set; }
        public int CFOP { get; set; }
        public string Unidade { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal AliquotaICMS { get; set; }
        public string CST { get; set; } = "00"; // Tributada integralmente

        public decimal ValorTotal => Quantidade * ValorUnitario;
        public decimal ValorBaseCalculo = 0;
        public decimal ValorICMS => ValorBaseCalculo * (AliquotaICMS / 100);

        public string cEAN { get; set; } = "SEM GTIN";

        public string cEANTrib { get; set; } = "SEM GTIN";





        // Método com parâmetro
        public det ToZeusModel()
        {
            return new det
            {
                nItem = NumeroItem,
                prod = new prod
                {
                    cProd = Codigo,
                    xProd = Descricao,
                    NCM = NCM,
                    CFOP = CFOP,
                    uCom = Unidade,
                    qCom = Quantidade,
                    vUnCom = ValorUnitario,
                    vProd = ValorTotal,
                    uTrib = Unidade,
                    qTrib = Quantidade,
                    vUnTrib = ValorTotal,
                    indTot = NFe.Classes.Informacoes.Detalhe.IndicadorTotal.ValorDoItemCompoeTotalNF,
                    cEAN = cEAN,
                    cEANTrib = cEANTrib
                },
                imposto = new imposto
                {
                    vTotTrib = ValorICMS,
                    ICMS = CriarICMS()
                }
            };
        }
        private ICMS CriarICMS()
        {
            // Aqui você pode adicionar lógica para diferentes CSTs
            return new ICMS
            {
                //TipoICMS = ,
                //TipoICMS = new ICMS00
                //{
                //    orig = OrigemMercadoria.OmNacional,
                //    CST = Csticms.Cst00,
                //    modBC = DeterminacaoBaseIcms.DbiValorOperacao,
                //    vBC = ValorBaseCalculo,
                //    pICMS = AliquotaICMS,
                //    vICMS = ValorICMS,
                    
                //}
                TipoICMS = new ICMSSN102
                {
                    orig = OrigemMercadoria.OmNacional,
                    CSOSN = Csosnicms.Csosn102
                }
            };
        }

    }
}
