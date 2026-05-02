using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using DFe.Utils;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Utils;
using NFe.Utils.Email;
using System.Net;

namespace NFCVeraConstrucoes
{

    public class ConfiguracaoNF 
    {
        // Certificado Digital
        public string CaminhoCertificado { get; set; }
        public string SenhaCertificado { get; set; }

        // Numeracao
        public int SerieNota { get; set; } = 1;
        public int UltimoNumeroNota { get; set; } 

        // Emitente
        public string CnpjEmitente { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string InscricaoEstadual { get; set; }
        public CRT RegimeTributario { get; set; } = CRT.SimplesNacionalMei;

        // Endereço Emitente
        public string LogradouroEmitente { get; set; }
        public string NumeroEmitente { get; set; }
        public string BairroEmitente { get; set; }
        public string ComplementoEmitente { get; set; }
        public int CodigoMunicipioIBGE { get; set; }
        public string NomeMunicipio { get; set; }
        public string UF { get; set; }
        public Estado Estado { get; set; }
        public string CEP { get; set; }
        public long Telefone { get; set; }
        public VersaoServico VersaoServico { get; set; } = VersaoServico.Versao400;


        public Estado UnidadeFederativaCodigo { get; set; }

        // Configuração SEFAZ
        public TipoAmbiente Ambiente { get; set; } = TipoAmbiente.Producao;
        public TipoEmissao TipoEmissao { get; set; } = TipoEmissao.teNormal;
        public string CSC { get; set; } // Código de Segurança do Contribuinte
        public string IdCSC { get; set; } // Identificador do CSC

        // Configurações de Timeout
        public int TimeoutTransmissao { get; set; } = 30; // segundos
        public int TentativasConsultaRecibo { get; set; } = 10;
        public int IntervaloConsultaRecibo { get; set; } = 3000; // milissegundos

        // Caminhos




        public string PastaXmlEnviados { get; set; } = "XMLs/Enviados";
        public string PastaXmlAutorizados { get; set; } = "XMLs/Autorizados";
        public string PastaXmlCancelados { get; set; } = "XMLs/Cancelados";
        public string PastaDanfes { get; set; } = "DANFEs";

        public emit Emitente { get; set; }
        public enderEmit EnderecoEmitente { get; set; }
        public ConfiguracaoEmail ConfiguracaoEmail { get; set; }

        private ConfiguracaoServico _cfgServico;

        public ConfiguracaoNF()
        {
            CfgServico = ConfiguracaoServico.Instancia;
            CfgServico.tpAmb = TipoAmbiente.Producao;
            CfgServico.ModeloDocumento = ModeloDocumento.NFCe;
            CfgServico.tpEmis = TipoEmissao.teNormal;
            CfgServico.ProtocoloDeSeguranca = ServicePointManager.SecurityProtocol;
            Emitente = new emit { CPF = "", CRT = CRT.SimplesNacional };
            EnderecoEmitente = new enderEmit();
            CfgServico.VersaoLayout = VersaoServico.Versao400;
            CfgServico.cUF = DFe.Classes.Entidades.Estado.SP;
            CfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //CfgServico.Certificado.Arquivo = @"C:\Users\wagne\Desktop\25806611.pfx";
            //CfgServico.Certificado.Senha = "326740";
            
            // ConfiguracaoEmail = new ConfiguracaoEmail("email@dominio.com", "senha", "Envio de NFE", Properties.Resources.MensagemHtml, "smtp.dominio.com", 587, true, true);
        }

        public ConfiguracaoServico CfgServico
        {
            get
            {
                ConfiguracaoServico.Instancia.CopiarPropriedades(_cfgServico);
                return _cfgServico;
            }
            set
            {
                _cfgServico = value;
                ConfiguracaoServico.Instancia.CopiarPropriedades(value);
            }
        }




        //// Certificado
        //public string CaminhoCertificado = @"C:\Users\wagne\Desktop\25806611.pfx";
        //public string SenhaCertificado = "326740";

        //// Numeracao
        //public int SerieNota { get; set; } = 1 ;
        //public int UltimoNumeroNota { get; set; } = 0;





        //// Emitente - ALTERE AQUI!
        //public static string CnpjEmitente = "63977431000122";
        //public static string RazaoSocial = "63.977.431 WAGNER MORAIS ARAUJO";
        ////public static string NomeFantasia = "NOME FANTASIA";
        //public static string InscricaoEstadual = "157099879111";
        //public static CRT RegimeTributario = CRT.SimplesNacionalMei;

        //// Endereço Emitente
        //public static string LogradouroEmitente = "RUA DESEMBARGADOR EWELSON SOARES PINTO";
        //public static string NumeroEmitente = "95";
        //public static string BairroEmitente = "VILA BOM JARDIM";
        //public static int CodigoMunicipioIBGE = 3550308;
        //public static string NomeMunicipio = "SAO PAULO";
        //public static string UF = "SP";
        //public static Estado Estado = DFe.Classes.Entidades.Estado.SP.SP;
        //public static string CEP = "04937200";

        //// Configuração SEFAZ
        //public static TipoAmbiente Ambiente = TipoAmbiente.Homologacao;
        //public static TipoEmissao TipoEmissao = TipoEmissao.teNormal;
        //public string CSC { get; set; } // Código de Segurança do Contribuinte
        //public string IdCSC { get; set; } // Identificador do CSC

        //// Configurações de Timeout
        //public int TimeoutTransmissao { get; set; } = 30; // segundos
        //public int TentativasConsultaRecibo { get; set; } = 10;
        //public int IntervaloConsultaRecibo { get; set; } = 3000; // milissegundos

        //// Caminhos
        //public string PastaXmlEnviados { get; set; } = "XMLs/Enviados";
        //public string PastaXmlAutorizados { get; set; } = "XMLs/Autorizados";
        //public string PastaXmlCancelados { get; set; } = "XMLs/Cancelados";
        //public string PastaDanfes { get; set; } = "DANFEs";

        // Produtos de exemplo (opcional)
        public static List<ProdutoExemplo> ProdutosExemplo = new List<ProdutoExemplo>
        {
            new ProdutoExemplo { Codigo = "001", Descricao = "PRODUTO TESTE 1", NCM = "21069090",
                CFOP = 5102, Valor = 50.00m, Quantidade = 2 },
            new ProdutoExemplo { Codigo = "002", Descricao = "PRODUTO TESTE 2", NCM = "99999999",
                CFOP = 5933, Valor = 30.00m, Quantidade = 1 }
        };

        public class ProdutoExemplo
        {
            public string Codigo { get; set; }
            public string Descricao { get; set; }
            public string NCM { get; set; }
            public int CFOP { get; set; }
            public decimal Valor { get; set; }
            public decimal Quantidade { get; set; }
        }
    }
}