using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Identificacao.Tipos;

namespace NFCVeraConstrucoes.Models.NFCe
{
    public class IdentificacaoNFCe
    {
        public int Serie { get; set; } = 1;
        public int NumeroNota { get; set; }
        public string NaturezaOperacao { get; set; } = "VENDA AO CONSUMIDOR";
        public Estado UF { get; set; }
        public TipoAmbiente Ambiente { get; set; }
        public TipoEmissao TipoEmissao { get; set; } = TipoEmissao.teNormal;

        public ide ToZeusModel(string cNf, TipoImpressao tipoImpressao = TipoImpressao.tiNFCe )
        {
            return new ide
            {
                cUF = UF,
                natOp = NaturezaOperacao,
                mod = ModeloDocumento.NFCe,
                serie = Serie,
                nNF = NumeroNota,
                dhEmi = DateTime.Now,
                //dhSaiEnt = DateTime.Now,
                tpNF = TipoNFe.tnSaida,
                tpEmis = TipoEmissao,
                tpAmb = Ambiente,
                indFinal = ConsumidorFinal.cfConsumidorFinal,
                indPres = PresencaComprador.pcPresencial,
                procEmi = ProcessoEmissao.peAplicativoContribuinte,
                verProc = "1.0",
                finNFe = FinalidadeNFe.fnNormal,
                idDest = DestinoOperacao.doInterna,
                cMunFG = 3550308,
                tpImp = tipoImpressao,
                cNF = cNf

            };
        }
    }
}
