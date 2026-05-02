using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using DFe.Utils;
using NFCVeraConstrucoes.Models;
using NFe.Classes;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Servicos;
using NFe.Utils;
using NFe.Utils.InformacoesSuplementares;
using NFe.Utils.NFe;
using System.Security.Cryptography;
using System.Text;
using System.Xml;



namespace NFCVeraConstrucoes.Services
{
    public class NFCService : IDisposable
    {

        private readonly ConfiguracaoServico _configServico;
        private readonly ConfiguracaoNFCe _configNFCe;
        private ServicosNFe _servicoNFe;
        private bool _disposed = false;

        public NFCService(ConfiguracaoNFCe configNFCe)
        {
            _configNFCe = configNFCe ?? throw new ArgumentNullException(nameof(configNFCe));


            _configServico = new ConfiguracaoServico
            {
                cUF = _configNFCe.Estado,
                tpAmb = _configNFCe.Ambiente,
                tpEmis = TipoEmissao.teNormal,
                ModeloDocumento = ModeloDocumento.NFCe,
                VersaoNFeAutorizacao = VersaoServico.Versao400,
                TimeOut = _configNFCe.TimeoutTransmissao * 1000,
                ValidarSchemas = false,
                
            };

            // Carregar certificado e configurar na instância
            //ConfigurarCertificado();
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

        public ResultadoEmissao EmitirNFCe(NFe.Classes.NFe nfce)
        {
            try
            {
                // 1. Validar NFCe
                var validacao = ValidarNFCe(nfce);
                if (!validacao.Sucesso)
                    return validacao;
                Console.WriteLine($"cDV antes assinatura: {nfce.infNFe.ide.cDV}");
                Console.WriteLine($"Id antes assinatura: {nfce.infNFe.Id}");

                string uf = _configNFCe.UF;
                //string chaveAcesso = nfce.infNFe.Id.Substring(3); // Remove "NFe" do início
                //int ambeinte = int.Parse(_configNFCe.Ambiente);

                //var QrCode = nfce.infNFeSupl.ObterUrlQrCode(nfce , VersaoQrCode.QrCodeVersao3, _configNFCe.IdCSC, _configNFCe.CSC, _configServico.Certificado);

                //CalcularHashQRCode(chaveAcesso, _configNFCe.CSC, _configNFCe.Ambiente, _configNFCe.IdCSC);
                

                // 2. Assinar
                AssinarNFCe(nfce);

                //nfce.Valida();

                nfce.infNFeSupl = new NFe.Classes.infNFeSupl();
                nfce.infNFeSupl.urlChave = nfce.infNFeSupl.ObterUrlConsulta(nfce, VersaoQrCode.QrCodeVersao2);
                nfce.infNFeSupl.qrCode = nfce.infNFeSupl.ObterUrlQrCode(nfce, VersaoQrCode.QrCodeVersao2, _configNFCe.IdCSC, _configNFCe.CSC, _configServico.Certificado);



                Console.WriteLine($"cDV após assinatura: {nfce.infNFe.ide.cDV}");
                Console.WriteLine($"Id após assinatura: {nfce.infNFe.Id}");

                

                // Exibir XML DEPOIS da assinatura
                Console.WriteLine("\n=== XML APÓS ASSINATURA ===");
                ExibirXmlNoConsole(nfce);

                // 3. Transmitir
                return TransmitirNFCe(nfce);

                //return ResultadoEmissao.SucessoCompleto("Certo", "", 0, 0);
            }
            catch (Exception ex)
            {
                return ResultadoEmissao.Erro($"Erro na emissão: {ex.Message}");
            }

        }

        private void ExibirXmlNoConsole(NFe.Classes.NFe nfce)
        {
            try
            {
                // Salvar em arquivo também para análise completa
                SalvarXmlArquivo(nfce);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir XML: {ex.Message}");
            }
        }

        private string FormatarXml(string xml)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\n",
                    NewLineHandling = NewLineHandling.Replace
                };

                using (var stringWriter = new StringWriter())
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    xmlDoc.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                    return stringWriter.GetStringBuilder().ToString();
                }
            }
            catch
            {
                return xml; // Retorna o XML original se não conseguir formatar
            }
        }

        private void SalvarXmlArquivo(NFe.Classes.NFe nfce)
        {
            try
            {
                // Criar pasta para XMLs
                string pastaXml = Path.Combine(Directory.GetCurrentDirectory(), "XMLs_Console");
                if (!Directory.Exists(pastaXml))
                    Directory.CreateDirectory(pastaXml);

                // Nome do arquivo com timestamp
                string nomeArquivo = $"NFCe_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                string caminhoCompleto = Path.Combine(pastaXml, nomeArquivo);

                // Salvar XML
                nfce.SalvarArquivoXml(caminhoCompleto);

                Console.WriteLine($"\n📁 XML salvo em: {caminhoCompleto}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Não foi possível salvar XML: {ex.Message}");
            }
        }

        private ResultadoEmissao ValidarNFCe(NFe.Classes.NFe nfce)
        {
            // Validações básicas
            if (nfce == null)
                return ResultadoEmissao.Erro("NFCe não fornecida");

            if (nfce.infNFe == null)
                return ResultadoEmissao.Erro("infNFe não configurada");

            if (nfce.infNFe.det == null || nfce.infNFe.det.Count == 0)
                return ResultadoEmissao.Erro("Nenhum produto informado");

            //if (nfce.infNFe.pag == null || nfce.infNFe.pag.detPag == null || nfce.infNFe.pag.detPag.Count == 0)
            //    return ResultadoEmissao.Erro("Nenhuma forma de pagamento informada");

            return ResultadoEmissao.SucessoCompleto("", "", 0,0);
        }

        private void AssinarNFCe(NFe.Classes.NFe nfce)
        {
            if (ConfiguracaoServico.Instancia.Certificado == null)
                throw new InvalidOperationException("Certificado não configurado para assinatura");

            nfce.Assina();
        }


        private ResultadoEmissao TransmitirNFCe(NFe.Classes.NFe nfce)
        {
            using (_servicoNFe = new ServicosNFe(_configServico))
            {

                // 1. Enviar para autorização
                var retornoAutorizacao = _servicoNFe.NFeAutorizacao(
                    idLote: GerarIdLote(),
                    indSinc: NFe.Classes.Servicos.Tipos.IndicadorSincronizacao.Sincrono,
                    nFes: new List<NFe.Classes.NFe> { nfce },
                    compactarMensagem: false

                );

             



                if (retornoAutorizacao.Retorno == null)
                    return ResultadoEmissao.Erro("Resposta da SEFAZ está vazia");

                // Verificar se já foi autorizada (sincrono)
                if (retornoAutorizacao.Retorno.protNFe.infProt.cMsg > 0)
                {
                    //var protocolo = retornoAutorizacao.Retorno.protNFe[0];
                    //return ProcessarAutorizacaoBemSucedida(nfce, protocolo);
                }

                // Se assíncrono, consultar recibo
                var numeroRecibo = retornoAutorizacao.Retorno.infRec?.nRec;
                if (string.IsNullOrEmpty(numeroRecibo))
                    return ResultadoEmissao.Erro("Número do recibo não retornado");

                return ConsultarRecibo(numeroRecibo, nfce);
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

        //private resultadoemissao ProcessarAutorizacaoBemSucedida(nfe.classes.nfe nfce, nfe.classes.servicos.autorizacao.envinfe4 protocolo)
        //{
        //    try
        //    {
        //        // salvar xmls
        //        salvarxmls(nfce, protocolo);

        //        // gerar danfe (opcional)
        //        var caminhodanfe = gerardanfe(nfce, protocolo);

        //        return new resultadoemissao
        //        {
        //            sucesso = true,
        //            mensagem = "nfce autorizada com sucesso",
        //            chaveacesso = protocolo.chnfe,
        //            numeroprotocolo = protocolo.nprot,
        //            datahoraautorizacao = protocolo.dhrecbto,
        //            caminhoxml = path.combine(_confignfce.pastaxmlautorizados, $"{protocolo.chnfe}.xml"),
        //            caminhodanfe = caminhodanfe
        //        };
        //    }
        //    catch (exception ex)
        //    {
        //        return resultadoemissao.erro($"erro ao processar autorização: {ex.message} " + ex);
        //    }
        //}

        private void SalvarXmls(NFe.Classes.NFe nfce, NFe.Classes.Servicos.Autorizacao.enviNFe4 protocolo)
        {
            //// Criar pastas se não existirem
            //Directory.CreateDirectory(_configNFCe.PastaXmlAutorizados);

            //// Salvar NFCe assinada
            //var caminhoNfce = Path.Combine(_configNFCe.PastaXmlAutorizados, $"{protocolo.}-nfe.xml");
            //nfce.SalvarXmlEmDisco(caminhoNfce);

            //// Salvar protocolo (simplificado - você pode implementar serialização completa)
            //var xmlProtocolo = $"<protNFe versao=\"4.00\"><infProt><tpAmb>{_configNFCe.Ambiente}</tpAmb><verAplic>1.0</verAplic><chNFe>{protocolo.chNFe}</chNFe><dhRecbto>{protocolo.dhRecbto:yyyy-MM-ddTHH:mm:sszzz}</dhRecbto><nProt>{protocolo.nProt}</nProt><digVal>{protocolo.digVal}</digVal><cStat>{protocolo.cStat}</cStat><xMotivo>{protocolo.xMotivo}</xMotivo></infProt></protNFe>";
            //File.WriteAllText(Path.Combine(_configNFCe.PastaXmlAutorizados, $"{protocolo.chNFe}-prot.xml"), xmlProtocolo);
        }

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
            return (int)DateTime.Now.Ticks % 1000000;
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
