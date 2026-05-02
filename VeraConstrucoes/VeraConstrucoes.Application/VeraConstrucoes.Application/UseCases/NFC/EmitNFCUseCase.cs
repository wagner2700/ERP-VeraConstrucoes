using DFe.Utils;
using DFe.Utils.Assinatura;
using NFCVeraConstrucoes;
using NFCVeraConstrucoes.Helpers;
using NFCVeraConstrucoes.Models;
using NFCVeraConstrucoes.Services;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using VeraConstrucoes.Application.AutoMapper;
using VeraConstrucoes.Application.UseCases.NFC.Interface;
using VeraConstrucoes.Communication.DTO;
using VeraConstrucoes.Communication.Request;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Application.UseCases.NFC
{
    public class EmitNFCUseCase  : IEmitNFCUseCase
    {

        private readonly ConfiguracaoCertificado _certificadoConfig;
        private readonly ISaveNFCUseCase _saveNFCUseCase;
        private readonly ConfiguracaoNF configuracaoNFCe;
        private readonly INFCeMapper _nfceMapper;
        private readonly CertificadoServices _certificadoServices;
        private readonly IConfiguracaoRepository _configuracaoRepository;
        private readonly INFCRepository _nfcRepository;


        public EmitNFCUseCase(ConfiguracaoCertificado configuracaoCertificado , ISaveNFCUseCase saveNFCUseCase , INFCeMapper nfceMapper, CertificadoServices certificado, IConfiguracaoRepository configuracaoRepository, INFCRepository nfcRepository)
        {
            _certificadoConfig = configuracaoCertificado;
            _saveNFCUseCase = saveNFCUseCase;
            _nfceMapper = nfceMapper;
            _configuracaoRepository = configuracaoRepository;
            _nfcRepository = nfcRepository;
            using (var certService = new CertificadoServices())
            {
                _certificadoConfig = certService.CarregarConfiguracaoCertificado(
                    ConfigHelper.CarregarConfiguracao().CaminhoCertificado,
                    ConfigHelper.CarregarConfiguracao().SenhaCertificado);

                var cnpj = certService.ExtrairCNPJCertificado();
                Console.WriteLine($"✓ Certificado carregado: {cnpj}");
            }
            _certificadoServices = certificado;

        }

        public async Task<RetornoRecepcaoEvento> CancelamentoNFCe(CancelamenoNotaFiscalRequest cancelamenoNota)
        {
            
            var configuracao = await _configuracaoRepository.ObterConfiguracaoAsync();

            using (var nfceService = new NFCService(ConfigHelper.CarregarConfiguracao(), _configuracaoRepository, _certificadoServices.CarregarCertificado(configuracao.CaminhoCertificado, configuracao.SenhaCertificado)))
            {

                var retorno =  nfceService.CancelamentoNfce(cancelamenoNota.protocoloAutorizacao, cancelamenoNota.chaveNfe, configuracao.CnpjEmitente);



                if(retorno.Retorno.retEvento.Count  > 0)
                {
                    foreach (var item in retorno.Retorno.retEvento)
                    {
                        if(item.infEvento.cStat == 135 || item.infEvento.cStat == 573 || item.infEvento.cStat == 101)
                        {
                            // Gravar situacao nota como cancelada
                            await _nfcRepository.UpdateSituacaoNotaFiscal(cancelamenoNota.idNota, "C");
                        }
                    }
                }
                return retorno;
            }
        }


        public async Task<ResultadoEmissao> Execute(NFCeRequestDto requestDto )
        {
            // Gravar nota fiscal
            var notaFiscal = await _saveNFCUseCase.GravarDadosIniciaisNotaFiscal(requestDto);

            // Obter configurações
            var configuracao = await _configuracaoRepository.ObterConfiguracaoAsync();
            using (var nfceService = new NFCService(ConfigHelper.CarregarConfiguracao(), _configuracaoRepository, _certificadoServices.CarregarCertificado(configuracao.CaminhoCertificado, configuracao.SenhaCertificado)))
            {
                // Converter DTO para NFe
                var nfce = await _nfceMapper.ToNFeEntity(requestDto);
                nfceService.ConfigurarCertificado(_certificadoConfig);

                var resultado = await nfceService.EmitirNFCe(nfce);
                resultado.Id = notaFiscal.Id;

                return resultado;
            }
        }

        public async Task DownloadXml(string chave)
        {
            using (var _certificado = CertificadoDigital.ObterCertificado(_certificadoConfig))
            using (var servicoNFe = new ServicosNFe(configuracaoNFCe.CfgServico, _certificado))
            {
                var retornoNFeDistDFe = servicoNFe.NfeDistDFeInteresse(ufAutor: configuracaoNFCe.EnderecoEmitente.UF.ToString(), documento: configuracaoNFCe.Emitente.CNPJ, chNFE: chave);
            }
        }



    }
}
