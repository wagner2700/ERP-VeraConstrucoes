using Microsoft.Extensions.Logging;
using NFCVeraConstrucoes;
using NFCVeraConstrucoes.Helpers;
using NFCVeraConstrucoes.Services;
using NFe.Classes.Servicos.Status;
using NFe.Servicos;
using System.Security.Cryptography.X509Certificates;
using VeraConstrucoes.Application.UseCases.NFC.Interface;

namespace VeraConstrucoes.Application.UseCases.NFC
{
    internal class SefazServiceUseCase : ISefazServiceUseCase
    {
        private readonly ConfiguracaoNF _config;
        private readonly ILogger<SefazServiceUseCase> _logger;
        private readonly CertificadoServices _certificadoServices;

        public SefazServiceUseCase(ILogger<SefazServiceUseCase> logger, CertificadoServices certificadoServices)
        {
            _logger = logger;
            //_config = ConfigHelper.CarregarConfiguracao();
            _certificadoServices = certificadoServices;


            // Configurar certificado CORRETAMENTE
            ConfigurarCertificado();


        }

        private void ConfigurarCertificado()
        {
            try
            {
                
                // Configurar certificado usando as propriedades padrão
                _config.CfgServico.Certificado = _certificadoServices.CarregarConfiguracaoCertificado(_config.CaminhoCertificado, _config.SenhaCertificado);
                

                _logger.LogInformation("Certificado configurado via CertificadoArquivo/CertificadoSenha");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao configurar certificado");
            }
        }

        public async Task<retConsStatServ> CheckStatusAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando consulta de status do serviço NFC-e");

                using (var servicoNFE = new ServicosNFe(_config.CfgServico))
                {
                    // O método retorna RetornoNfeStatusServico, precisamos extrair retConsStatServ
                    var retornoStatus = servicoNFE.NfeStatusServico();

                    // Acessamos a propriedade retConsStatServ
                    var resultado = retornoStatus.Retorno;

                    _logger.LogInformation($"Status consultado: {resultado.cStat} - {resultado.xMotivo}");

                    return resultado;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar status do serviço NFC-e");
                throw;
            }
        }

        // Método simples para testar conexão
        public bool TestarConexaoSefaz()
        {
            try
            {
                using (var servicoNFE = new ServicosNFe(_config.CfgServico))
                {
                    var retornoStatus = servicoNFE.NfeStatusServico();
                    return retornoStatus.Retorno.cStat == 107; // 107 = Serviço em Operação
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task BaixarNotaFiscal(string chave)
        {

        }
    }
}