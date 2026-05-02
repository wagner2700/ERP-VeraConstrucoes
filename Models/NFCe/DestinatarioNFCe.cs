


using DFe.Classes.Flags;
using NFe.Classes.Informacoes.Destinatario;

namespace NFCVeraConstrucoes.Models.NFCe
{
    public class DestinatarioNFCe
    {
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public indIEDest IndicadorIE { get; set; } = indIEDest.NaoContribuinte;

        

        
        public dest ToZeusModel(VersaoServico versao)
        {
            var destinatario = new dest(versao)
            {
                xNome = Nome,
                indIEDest = IndicadorIE
            };

            if (!string.IsNullOrEmpty(CPF))
                destinatario.CPF = CPF;
            else if (!string.IsNullOrEmpty(CNPJ))
                destinatario.CNPJ = CNPJ;

            return destinatario;
        }

        public static DestinatarioNFCe Destinatario()
        {
            return new DestinatarioNFCe
            {
                Nome = "CONSUMIDOR NAO IDENTIFICADO",
                IndicadorIE = indIEDest.NaoContribuinte
            };
        }

        
    }
}
