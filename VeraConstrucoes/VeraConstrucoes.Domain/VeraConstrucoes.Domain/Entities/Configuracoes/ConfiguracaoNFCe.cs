using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using System.ComponentModel.DataAnnotations;

namespace VeraConstrucoes.Domain.Entities.Configuracoes
{
    public class ConfiguracaoNFCe
    {
        [Key]
        public int Id { get; set; } = 1; // Sempre 1 para haver apenas um registro

        [MaxLength(500)]
        public string CaminhoCertificado { get; set; } = string.Empty;

        [MaxLength(100)]
        public string SenhaCertificado { get; set; } = string.Empty;

        public int SerieNota { get; set; }

        public int UltimoNumeroNota { get; set; }

        [MaxLength(20)]
        public string CnpjEmitente { get; set; } = string.Empty;

        [MaxLength(150)]
        public string RazaoSocial { get; set; } = string.Empty;

        [MaxLength(150)]
        public string NomeFantasia { get; set; } = string.Empty;

        [MaxLength(20)]
        public string InscricaoEstadual { get; set; } = string.Empty;

        public CRT RegimeTributario { get; set; } 

        [MaxLength(200)]
        public string LogradouroEmitente { get; set; } = string.Empty;

        [MaxLength(20)]
        public string NumeroEmitente { get; set; } = string.Empty;

        [MaxLength(100)]
        public string BairroEmitente { get; set; } = string.Empty;

        [MaxLength(50)]
        public string ComplementoEmitente { get; set; } = string.Empty;

        public int CodigoMunicipioIBGE { get; set; }

        [MaxLength(100)]
        public string NomeMunicipio { get; set; } = string.Empty;

        [MaxLength(2)]
        public string UF { get; set; } = string.Empty;

        public Estado Estado { get; set; } // código IBGE da UF

        [MaxLength(10)]
        public string CEP { get; set; } = string.Empty;

        public int NumeroLote {  get; set; } 

        
        public long Telefone { get; set; } = 0;

        public int UnidadeFederativaCodigo { get; set; } // redundante, pode ser o mesmo que Estado

        public TipoAmbiente Ambiente { get; set; } // 1-produção, 2-homologação

        public TipoEmissao TipoEmissao { get; set; } = TipoEmissao.teNormal;

        [MaxLength(100)]
        public string CSC { get; set; } = string.Empty;

        [MaxLength(20)]
        public string IdCSC { get; set; } = string.Empty;

        public int TimeoutTransmissao { get; set; }

        public int TentativasConsultaRecibo { get; set; }

        public int IntervaloConsultaRecibo { get; set; }

        [MaxLength(250)]
        public string PastaXmlEnviados { get; set; } = string.Empty;

        [MaxLength(250)]
        public string PastaXmlAutorizados { get; set; } = string.Empty;

        [MaxLength(250)]
        public string PastaXmlCancelados { get; set; } = string.Empty;

        [MaxLength(250)]
        public string PastaDanfes { get; set; } = string.Empty;

        
        public VersaoServico Versao { get; set; } 
    }
}
