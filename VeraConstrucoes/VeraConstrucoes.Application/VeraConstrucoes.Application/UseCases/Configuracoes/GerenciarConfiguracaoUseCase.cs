using VeraConstrucoes.Application.UseCases.Configuracoes.Interface;
using VeraConstrucoes.Domain.Entities.Configuracoes;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Application.UseCases.Configuracoes
{
    public class GerenciarConfiguracaoUseCase : IGerenciarConfiguracaoUseCase
    {
        private readonly IConfiguracaoRepository _repository;

        public GerenciarConfiguracaoUseCase(IConfiguracaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ConfiguracaoNFCe?> ObterConfiguracao()
        {
            return await _repository.ObterConfiguracaoAsync();
        }

        

        public async Task<ConfiguracaoNFCe> SalvarConfiguracao(ConfiguracaoNFCe config)
        {
            var existente = await _repository.ObterConfiguracaoAsync();
            if (existente == null)
            {
                config.Id = 1;
                await _repository.InserirConfiguracaoAsync(config);
            }
            else
            {
                // Atualiza todas as propriedades (pode usar mapeamento automático)
                existente.CaminhoCertificado = config.CaminhoCertificado;
                existente.SenhaCertificado = config.SenhaCertificado;
                existente.SerieNota = config.SerieNota;
                existente.UltimoNumeroNota = config.UltimoNumeroNota;
                existente.CnpjEmitente = config.CnpjEmitente;
                existente.RazaoSocial = config.RazaoSocial;
                existente.NomeFantasia = config.NomeFantasia;
                existente.InscricaoEstadual = config.InscricaoEstadual;
                existente.RegimeTributario = config.RegimeTributario;
                existente.LogradouroEmitente = config.LogradouroEmitente;
                existente.NumeroEmitente = config.NumeroEmitente;
                existente.BairroEmitente = config.BairroEmitente;
                existente.ComplementoEmitente = config.ComplementoEmitente;
                existente.CodigoMunicipioIBGE = config.CodigoMunicipioIBGE;
                existente.NomeMunicipio = config.NomeMunicipio;
                existente.UF = config.UF;
                existente.Estado = config.Estado;
                existente.CEP = config.CEP;
                existente.Telefone = config.Telefone;
                existente.UnidadeFederativaCodigo = config.UnidadeFederativaCodigo;
                existente.Ambiente = config.Ambiente;
                existente.TipoEmissao = config.TipoEmissao;
                existente.CSC = config.CSC;
                existente.IdCSC = config.IdCSC;
                existente.TimeoutTransmissao = config.TimeoutTransmissao;
                existente.TentativasConsultaRecibo = config.TentativasConsultaRecibo;
                existente.IntervaloConsultaRecibo = config.IntervaloConsultaRecibo;
                existente.PastaXmlEnviados = config.PastaXmlEnviados;
                existente.PastaXmlAutorizados = config.PastaXmlAutorizados;
                existente.PastaXmlCancelados = config.PastaXmlCancelados;
                existente.PastaDanfes = config.PastaDanfes;
                existente.Versao = config.Versao;

                await _repository.AtualizarConfiguracaoAsync(existente);
                config = existente;
            }
            return config;
        }
    }
}
