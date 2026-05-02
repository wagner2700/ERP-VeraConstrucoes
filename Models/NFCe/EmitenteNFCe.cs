using NFe.Classes.Informacoes.Emitente;

namespace NFCVeraConstrucoes.Models.NFCe
{
    public class EmitenteNFCe
    {
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string InscricaoEstadual { get; set; }
        public CRT RegimeTributario { get; set; } = CRT.SimplesNacional;

        // Endereço
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public int CodigoMunicipioIBGE { get; set; }
        public string NomeMunicipio { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }

        public emit ToZeusModel()
        {
            return new emit
            {
                CNPJ = CNPJ,
                xNome = RazaoSocial,
                xFant = NomeFantasia,
                IE = InscricaoEstadual,
                CRT = RegimeTributario,
                enderEmit = new enderEmit
                {
                    xLgr = Logradouro,
                    nro = Numero,
                    xBairro = Bairro,
                    cMun = CodigoMunicipioIBGE,
                    xMun = NomeMunicipio,
                    UF = DFe.Classes.Entidades.Estado.SP,
                    CEP = CEP,
                    cPais = 1058,
                    xPais = "BRASIL"
                }
            };
        }
    }
}
