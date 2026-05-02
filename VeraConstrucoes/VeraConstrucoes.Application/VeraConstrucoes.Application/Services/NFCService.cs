using DFe.Classes.Flags;
using DFe.Utils;
using NFCVeraConstrucoes.Models;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Servicos.DistribuicaoDFe.Schemas;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using NFe.Utils;
using NFe.Utils.InformacoesSuplementares;
using NFe.Utils.NFe;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VeraConstrucoes.Application.Services;
using VeraConstrucoes.Domain.Entities.Configuracoes;
using VeraConstrucoes.Infrastructure.Repositories;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;



namespace NFCVeraConstrucoes.Services
{
     
    public class NFCService : IDisposable, INFCService
    {

        private readonly ConfiguracaoServico _configServico;
        private readonly ConfiguracaoNF _configNFCe;
        private readonly IConfiguracaoRepository _configuracaoRepository;
        private ServicosNFe _servicoNFe;
        private bool _disposed = false;
        private readonly string diretorioSchema;
        private readonly INFCRepository _nfcRepository;


        public NFCService(INFCRepository nfcRepository , IConfiguracaoRepository configuracaoRepository )
        {
            _nfcRepository = nfcRepository;
            _configuracaoRepository = configuracaoRepository;
        }


        public NFCService(ConfiguracaoNF configNFCe, IConfiguracaoRepository configuracaoRepository, X509Certificate2 certificado = null)
        {
            string baseDir = AppContext.BaseDirectory;
            diretorioSchema = Path.Combine(baseDir, @"Schemas\PL_010b_NT2025_002_v1.30");
            _configNFCe = configNFCe ?? throw new ArgumentNullException(nameof(configNFCe));
            _configuracaoRepository = configuracaoRepository;

            _configServico = new ConfiguracaoServico
            {
                cUF = _configNFCe.Estado,
                tpAmb = _configNFCe.Ambiente,
                tpEmis = TipoEmissao.teNormal,
                ModeloDocumento = ModeloDocumento.NFCe,
                VersaoNFeAutorizacao = VersaoServico.Versao400,
                TimeOut = _configNFCe.TimeoutTransmissao * 1000,
                ValidarSchemas = true,
                DiretorioSchemas = diretorioSchema,
                VersaoNFeRetAutorizacao = VersaoServico.Versao400,
                VersaoRecepcaoEventoCceCancelamento = VersaoServico.Versao400,
                
                

            };
            // Se o certificado foi passado, usa ele; caso contrário, tenta carregar da config
            if (certificado != null)
                _servicoNFe = new ServicosNFe(_configServico, certificado);
            else
                _servicoNFe = new ServicosNFe(_configServico);
        }

        public void ConfigurarCertificado(ConfiguracaoCertificado certificadoConfig)
        {
            if (certificadoConfig == null)
                throw new ArgumentNullException(nameof(certificadoConfig));

            // Configurar na instância estática para assinatura
            ConfiguracaoServico.Instancia.Certificado = certificadoConfig;

            // Configurar na instância local para transmissão
            _configServico.Certificado = certificadoConfig;
        }

        public async Task< ResultadoEmissao> EmitirNFCe(NFe.Classes.NFe nfce)
        {
            try
            {
                

                // 2. Assinar
                AssinarNFCe(nfce);

                nfce.infNFeSupl = new NFe.Classes.infNFeSupl();
                var versaoQrCode = (nfce.infNFe.dest == null )
                   ? VersaoQrCode.QrCodeVersao3
                   : VersaoQrCode.QrCodeVersao2;


                nfce.infNFeSupl.urlChave = nfce.infNFeSupl.ObterUrlConsulta(nfce, versaoQrCode);
                nfce.infNFeSupl.qrCode = nfce.infNFeSupl.ObterUrlQrCode(nfce, versaoQrCode, _configNFCe.IdCSC, _configNFCe.CSC, _configServico.Certificado);


                //ExibirXmlNoConsole(nfce);
                // Gravar Nota Fiscal
                

                var caminhoArquivo = SalvarXmlArquivo(nfce);
                nfce.Valida(_configServico);
                // 3. Transmitir
                var resultado = await TransmitirNFCe(nfce);

                resultado.XmlPath = caminhoArquivo;
                resultado.QrCodeUrl = nfce.infNFeSupl.qrCode;
                return resultado;
                //return ResultadoEmissao.SucessoCompleto("Certo", "", 0, 0);
            }
            catch (Exception ex)
            {
                return ResultadoEmissao.Erro($"Erro na emissão: {ex.Message}");
            }

        }


        

       

        private string SalvarXmlArquivo(NFe.Classes.NFe nfce)
        {
            try
            {
                // Criar pasta para XMLs
                string pastaXml = ObterDiretorioXml();
                if (!Directory.Exists(pastaXml))
                    Directory.CreateDirectory(pastaXml);

                // Nome do arquivo com timestamp
                var chaveNotaFiscal =  nfce.infNFe.Id.Substring(3);
                string nomeArquivo = $"{chaveNotaFiscal}.xml";
                string caminhoCompleto = Path.Combine(pastaXml, nomeArquivo);

                // Salvar XML
                nfce.SalvarArquivoXml(caminhoCompleto);
                Console.WriteLine($"\n📁 XML salvo em: {caminhoCompleto}");

                return caminhoCompleto;
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Não foi possível salvar XML: {ex.Message}");
                return string.Empty;
            }
        }
        private string ObterDiretorioXml()
        {
            var diretorioConfigurado = Environment.GetEnvironmentVariable("NFC_XML_DIR");

            if (!string.IsNullOrWhiteSpace(diretorioConfigurado))
                return diretorioConfigurado;

            return Path.Combine(Directory.GetCurrentDirectory(), "XMLs_Console");
        }

        

        private void AssinarNFCe(NFe.Classes.NFe nfce)
        {
            if (ConfiguracaoServico.Instancia.Certificado == null)
                throw new InvalidOperationException("Certificado não configurado para assinatura");

            nfce.Assina();
        }


        public RetornoRecepcaoEvento CancelamentoNfce(string protocoloAutorizacao , string chaveNfe , string cnpj)
        {            
            return _servicoNFe.RecepcaoEventoCancelamento(1 , 1, protocoloAutorizacao, chaveNfe, "Cancelamento de teste", cnpj);

        }


        private async Task<ResultadoEmissao> TransmitirNFCe(NFe.Classes.NFe nfce)
        {
            using (_servicoNFe = new ServicosNFe(_configServico))
            {
                
                // 1. Enviar para autorização
                var retornoAutorizacao = _servicoNFe.NFeAutorizacao(
                    idLote: await _configuracaoRepository.ObterProximoLote(),
                    indSinc: NFe.Classes.Servicos.Tipos.IndicadorSincronizacao.Sincrono,
                    nFes: new List<NFe.Classes.NFe> { nfce },
                    compactarMensagem: false
                );

                

                var retEnviNFe = retornoAutorizacao.Retorno;

                switch (retEnviNFe.protNFe.infProt.cStat)
                {
                    case 100:  // Autorizado
                    case 150:  // Autorizado fora do prazo
                    case 104:
                        if (retEnviNFe.protNFe != null)
                        {
                            // Verifique se protNFe é uma lista ou objeto único.
                            // Se for lista (ex: List<protNFe>), use:
                            // var protocolo = retEnviNFe.protNFe.FirstOrDefault();
                            // Se for objeto único, use:
                            var protocolo = retEnviNFe.protNFe;

                            if (protocolo != null)
                                return ProcessarAutorizacaoBemSucedida(nfce, protocolo);
                            else
                                return ResultadoEmissao.Erro("Protocolo não retornado mesmo com autorização informada.");
                        }
                        else
                        {
                            return ResultadoEmissao.Erro("Protocolo não retornado mesmo com autorização informada.");
                        }

                    case 103: // Lote recebido com sucesso (processamento assíncrono)
                        if (!string.IsNullOrEmpty(retEnviNFe.infRec?.nRec))
                        {
                            return ConsultarRecibo(retEnviNFe.infRec.nRec, nfce);
                        }
                        else
                        {
                            return ResultadoEmissao.Erro("Número do recibo não retornado para processamento assíncrono.");
                        }

                    default: // Qualquer outro código (rejeição, erro)
                        string mensagemErro = $"SEFAZ retornou código {retEnviNFe.protNFe.infProt.cStat}: {retEnviNFe.protNFe.infProt.xMotivo}";
                        return ResultadoEmissao.Erro(mensagemErro);
                }

            }
        }

        private ResultadoEmissao ConsultarRecibo(string numeroRecibo, NFe.Classes.NFe nfce)
        {
            for (int tentativa = 1; tentativa <= _configNFCe.TentativasConsultaRecibo; tentativa++)
            {
                try
                {
                    Thread.Sleep(_configNFCe.IntervaloConsultaRecibo);

                    var retornoRecibo = _servicoNFe.NFeRetAutorizacao(numeroRecibo);

                    if (retornoRecibo.Retorno?.protNFe?.Count > 0)
                    {
                        var protocolo = retornoRecibo.Retorno.protNFe[0];
                        //return ProcessarAutorizacaoBemSucedida(nfce, protocolo);
                    }

                    // Verificar status do processamento
                    if (retornoRecibo.Retorno?.cStat == 105) // Lote em processamento
                        continue;

                    if (!string.IsNullOrEmpty(retornoRecibo.Retorno?.cStat.ToString()))
                    {
                        return ResultadoEmissao.Erro(
                            retornoRecibo.Retorno.cStat + " - " +
                            retornoRecibo.Retorno.xMotivo);
                    }
                }
                catch (Exception ex)
                {
                    return ResultadoEmissao.Erro($"Erro na consulta do recibo: {ex.Message}" + ex);
                }
            }

            return ResultadoEmissao.Erro("Tempo esgotado para consulta do recibo");
        }

        private ResultadoEmissao ProcessarAutorizacaoBemSucedida(NFe.Classes.NFe nfce, NFe.Classes.Protocolo.protNFe protocolo )
        {
            try
            {
                //salvar xmls
                //SalvarXmls(nfce, protocolo);

                //gerar danfe(opcional)
                //var caminhodanfe = gerardanfe(nfce, protocolo);


                return new ResultadoEmissao()
                {
                    Sucesso = true,
                    Mensagem = protocolo.infProt.xMsg,
                    ChaveAcesso = protocolo.infProt.chNFe,
                    Protocolo = protocolo.infProt.nProt,
                    MensagemErro = protocolo.infProt.xMotivo,
                    Numero = nfce.infNFe.ide.nNF,
                    Serie = nfce.infNFe.ide.serie
                };
                
            }
            catch (Exception ex)
            {
                return ResultadoEmissao.Erro($"erro ao processar autorização: {ex.Message} " + ex);
            }
        }

        //private void SalvarXmls(NFe.Classes.NFe nfce, NFe.Classes.Servicos.Autorizacao.enviNFe4 protocolo)
        //{
        //    // Criar pastas se não existirem
        //    Directory.CreateDirectory(_configNFCe.PastaXmlAutorizados);

        //    // Salvar NFCe assinada
        //    var caminhoNfce = Path.Combine(_configNFCe.PastaXmlAutorizados, $"{protocolo.}-nfe.xml");
        //    nfce.SalvarXmlEmDisco(caminhoNfce);

        //    // Salvar protocolo (simplificado - você pode implementar serialização completa)
        //    var xmlProtocolo = $"<protNFe versao=\"4.00\"><infProt><tpAmb>{_configNFCe.Ambiente}</tpAmb><verAplic>1.0</verAplic><chNFe>{protocolo.chNFe}</chNFe><dhRecbto>{protocolo.dhRecbto:yyyy-MM-ddTHH:mm:sszzz}</dhRecbto><nProt>{protocolo.nProt}</nProt><digVal>{protocolo.digVal}</digVal><cStat>{protocolo.cStat}</cStat><xMotivo>{protocolo.xMotivo}</xMotivo></infProt></protNFe>";
        //    File.WriteAllText(Path.Combine(_configNFCe.PastaXmlAutorizados, $"{protocolo.chNFe}-prot.xml"), xmlProtocolo);
        //}

        //private string GerarDanfe(NFe nfce, NFe.Classes.Servicos.Autorizacao.protNFe protocolo)
        //{
        //    try
        //    {
        //        Directory.CreateDirectory(_configNFCe.PastaDanfes);

        //        // Implementar geração de DANFE aqui
        //        // Você pode usar NFe.Utils.Danfe.DanfeFrNfce
        //        var caminhoDanfe = Path.Combine(_configNFCe.PastaDanfes, $"{protocolo.chNFe}.pdf");

        //        // Exemplo simplificado:
        //        // var danfe = new DanfeFrNfce(nfce, configDanfe, _configNFCe.CSC, _configNFCe.IdCSC);
        //        // danfe.Visualizar(); // ou .Salvar(caminhoDanfe);

        //        return caminhoDanfe;
        //    }
        //    catch
        //    {
        //        return null; // DANFE não é crítico
        //    }
        //}

        private int GerarIdLote()
        {
            return (int)(DateTime.Now.Ticks % 999_999) + 1; // 1 a 999.999
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _servicoNFe?.Dispose();
                }
                _disposed = true;
            }
        }

        public static string CalcularHashQRCode(string chaveAcesso, string cscToken, TipoAmbiente tipoAmbiente, string cscId)
        {
            try
            {
                // 1. Montar a string de dados conforme o padrão da SEFAZ
                // Formato: CHAVE|VERSAO_QRCODE|TP_AMB|CSC_ID
                string dadosParaHash = $"{chaveAcesso}|2|{tipoAmbiente}|{cscId}";

                // 2. Converter o CSC Token (chave) e os dados para bytes
                byte[] chaveBytes = Encoding.UTF8.GetBytes(cscToken);
                byte[] dadosBytes = Encoding.UTF8.GetBytes(dadosParaHash);

                // 3. Calcular o HMAC-SHA256
                using (var hmac = new HMACSHA256(chaveBytes))
                {
                    byte[] hashBytes = hmac.ComputeHash(dadosBytes);

                    // 4. Converter para hexadecimal (MAIÚSCULAS)
                    return BitConverter.ToString(hashBytes)
                        .Replace("-", "")
                        .ToUpper();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Falha ao calcular hash do QR-Code: {ex.Message}", ex);
            }
        }



    }
}
