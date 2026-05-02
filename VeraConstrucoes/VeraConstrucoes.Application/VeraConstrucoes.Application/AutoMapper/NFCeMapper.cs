using DFe.Classes.Flags;
using NFCVeraConstrucoes;
using NFCVeraConstrucoes.Helpers;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Informacoes.Pagamento;
using VeraConstrucoes.Application.UseCases.Configuracoes.Interface;
using VeraConstrucoes.Communication.DTO;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Application.AutoMapper
{
    public class NFCeMapper : INFCeMapper
    {
        private  readonly INCMRepository _NCMRepository;
        private readonly IGerenciarConfiguracaoUseCase _gerenciarConfiguracaoUseCase;
        private readonly IConfiguracaoRepository _configuracaoRepository;
        

        public NFCeMapper(INCMRepository NCMRepository, IGerenciarConfiguracaoUseCase gerenciarConfiguracaoUseCase, IConfiguracaoRepository configuracaoRepository)
        {
            _NCMRepository = NCMRepository;
            _gerenciarConfiguracaoUseCase = gerenciarConfiguracaoUseCase;
            _configuracaoRepository = configuracaoRepository;

            
        }
        public async Task< NFe.Classes.NFe> ToNFeEntity(NFCeRequestDto dto)
        {
            var configuracao = await _gerenciarConfiguracaoUseCase.ObterConfiguracao();

            var nfce = new NFe.Classes.NFe();

            // Gerar Numero CNF - Código Numérico
            Random rand = new Random();
            int codigoNumerico = rand.Next(1, 99999999);
            string cNF = codigoNumerico.ToString("D8");
            var numeroNota = await _configuracaoRepository.ObterProximoNumeroNota();

            // Preencher dados do emitente (da configuração)
            nfce.infNFe = new NFe.Classes.Informacoes.infNFe()
            {
                versao =  "4.00"
            };
            nfce.infNFe.ide = new NFe.Classes.Informacoes.Identificacao.ide
            {
                cUF = configuracao.Estado,
                natOp = "VENDA DE MERCADORIA",
                mod = ModeloDocumento.NFCe,
                serie = configuracao.SerieNota,
                nNF = numeroNota,
                dhEmi = DateTime.Now,
                tpNF = TipoNFe.tnSaida, // Saída
                idDest = DestinoOperacao.doInterna, // Operação interna
                tpEmis = configuracao.TipoEmissao, // Normal
                tpAmb = configuracao.Ambiente,
                finNFe = FinalidadeNFe.fnNormal, // Normal
                indFinal = ConsumidorFinal.cfConsumidorFinal, // Consumidor final
                indPres = PresencaComprador.pcPresencial, // Presencial
                procEmi = 0, // Aplicativo do contribuinte
                verProc = "1.0.0",
                cMunFG = configuracao.CodigoMunicipioIBGE,
                cNF = cNF,
                tpImp = TipoImpressao.tiNFCe,
               
            };
            // Emitente
            nfce.infNFe.emit = new NFe.Classes.Informacoes.Emitente.emit
            {
                CNPJ = configuracao.CnpjEmitente,
                xNome = configuracao.RazaoSocial,
                xFant = configuracao.NomeFantasia,
                enderEmit = new NFe.Classes.Informacoes.Emitente.enderEmit
                {
                    xLgr = configuracao.LogradouroEmitente,
                    nro = configuracao.NumeroEmitente,
                    xBairro = configuracao.BairroEmitente,
                    cMun = configuracao.CodigoMunicipioIBGE,
                    xMun = configuracao.NomeMunicipio,
                    UF = configuracao.Estado,
                    CEP = configuracao.CEP,
                    cPais = 1058,
                    xPais = "BRASIL",
                    fone = configuracao.Telefone
                },
                IE = configuracao.InscricaoEstadual,
                CRT = configuracao.RegimeTributario
            };

            // Destinatário (cliente)
            if (dto.Cliente != null && dto.Cliente.TipoDocumento != "anonimo")
            {
                nfce.infNFe.dest = new NFe.Classes.Informacoes.Destinatario.dest(configuracao.Versao)
                {
                    xNome = nfce.infNFe.ide.tpAmb == TipoAmbiente.Homologacao? "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL" :  dto.Cliente.Nome,
                };

                if (dto.Cliente.TipoDocumento == "cpf")
                {
                    nfce.infNFe.dest.CPF = dto.Cliente.Documento;
                    nfce.infNFe.dest.indIEDest = NFe.Classes.Informacoes.Destinatario.indIEDest.NaoContribuinte;
                }
                else if (dto.Cliente.TipoDocumento == "cnpj")
                {
                    nfce.infNFe.dest.CNPJ = dto.Cliente.Documento;
                }
            }

            // Produtos
            nfce.infNFe.det = new List<NFe.Classes.Informacoes.Detalhe.det>();
            
            for (int i = 0; i < dto.Produtos.Count; i++)
            {
                var produtoDto = dto.Produtos[i];
                var det = new NFe.Classes.Informacoes.Detalhe.det
                {
                    nItem = i + 1,
                    prod = new NFe.Classes.Informacoes.Detalhe.prod
                    {
                        cProd = produtoDto.id.ToString(),
                        xProd = produtoDto.Descricao,
                        NCM = produtoDto.NCM,
                        CFOP = produtoDto.CFOP,
                        uCom = produtoDto.Unidade,
                        qCom = produtoDto.Quantidade,
                        vUnCom = produtoDto.ValorUnitario,
                        vProd = produtoDto.Quantidade * produtoDto.ValorUnitario,
                        indTot = NFe.Classes.Informacoes.Detalhe.IndicadorTotal.ValorDoItemCompoeTotalNF, // Valor total do item
                        uTrib = produtoDto.Unidade,
                        qTrib = produtoDto.Quantidade,
                        vUnTrib = produtoDto.ValorUnitario,
                        cEAN = "SEM GTIN",
                        cEANTrib = "SEM GTIN",
                        
                        

                    }
                };
                var ncm = await _NCMRepository.GetDadosNcm(produtoDto.NCM);
                //var configuracao = ConfigHelper.CarregarConfiguracao();
                switch (ncm.csosn)
                {
                    case 102 :
                        if(configuracao.RegimeTributario == CRT.SimplesNacional || configuracao.RegimeTributario == CRT.SimplesNacionalMei)
                        {
                            det.imposto = new NFe.Classes.Informacoes.Detalhe.Tributacao.imposto
                            {

                                ICMS = new ICMS
                                {
                                    TipoICMS = new ICMSSN102
                                    {
                                        orig = OrigemMercadoria.OmNacional,
                                        CSOSN = Csosnicms.Csosn102
                                    }
                                },
                                PIS = new PIS
                                {
                                    TipoPIS = new PISOutr { CST = CSTPIS.pis99, pPIS = 0, vBC = 0, vPIS = 0 }
                                },
                                COFINS = new COFINS
                                {
                                    TipoCOFINS = new COFINSOutr { CST = CSTCOFINS.cofins99, pCOFINS = 0, vBC = 0, vCOFINS = 0 }
                                }

                            };
                        }
                        
                        break;
                }
                nfce.infNFe.det.Add(det);
            }

            // Transporte
            nfce.infNFe.transp = new NFe.Classes.Informacoes.Transporte.transp
            {
                modFrete = NFe.Classes.Informacoes.Transporte.ModalidadeFrete.mfSemFrete
            };

            // Total
            nfce.infNFe.total = new NFe.Classes.Informacoes.Total.total
            {
                ICMSTot = new NFe.Classes.Informacoes.Total.ICMSTot
                {
                    vBC = 0, // dto.Produtos.Sum(p => p.Quantidade * p.ValorUnitario),
                    vICMS = 0 ,//dto.Produtos.Sum(p => (p.Quantidade * p.ValorUnitario) * (p.ICMS ?? 7.0m) / 100),
                    vProd = dto.Produtos.Sum(p => p.Quantidade * p.ValorUnitario),
                    vNF = dto.Produtos.Sum(p => p.Quantidade * p.ValorUnitario),
                    vICMSDeson = 0,
                    vFCP = 0,
                    vFCPST = 0,
                    vFCPSTRet = 0,
                    vFrete = 0,
                    vSeg = 0,
                    vDesc = 0,
                    vII = 0,
                    vIPI = 0,
                    vPIS = 0,
                    vCOFINS = 0,
                    vOutro = 0,
                    vIPIDevol = 0,
                    vTotTrib = 0
                }
            };

            // Pagamento
            nfce.infNFe.pag = new List<pag>();

            // Cria um grupo de pagamento (pode haver mais de um se houver diferentes formas)
            // Na NFC-e, o grupo <pag> pode conter vários <detPag> para diferentes formas de pagamento na mesma transação.
            // Normalmente usa-se um único <pag> com múltiplos <detPag>.
            var pagamentoGrupo = new pag
            {
                detPag = new List<detPag>()
            };
            decimal somaPagamentos = 0;

            foreach (var pagDto in dto.Pagamentos)
            {
                var detPag = new detPag
                {
                    tPag = ObterCodigoPagamento(pagDto.metodo),
                    vPag = pagDto.Valor
                };

                // Se for pagamento com cartão, pode ser necessário preencher informações adicionais
                if (detPag.tPag == FormaPagamento.fpCartaoCredito || detPag.tPag == FormaPagamento.fpCartaoDebito)
                {
                    detPag.card = new card
                    {
                        tpIntegra =  TipoIntegracaoPagamento.TipNaoIntegrado,
                                       // cAut = null, // autorização, se houver
                                       // CNPJ = "cnpj da credenciadora" // opcional
                    };
                }

                pagamentoGrupo.detPag.Add(detPag);
                somaPagamentos += pagDto.Valor;
            }
            // Validação simples: a soma dos pagamentos deve ser igual ao valor total da nota
            if (Math.Abs(somaPagamentos - nfce.infNFe.total.ICMSTot.vNF) > 0.01m)
            {
                // Aqui você pode lançar uma exceção ou ajustar a lógica, pois a NFC-e exige que o total pago seja igual ao valor da nota.
                throw new InvalidOperationException("A soma dos pagamentos não corresponde ao valor total da nota.");
            }
            nfce.infNFe.pag.Add(pagamentoGrupo);
            return nfce;
        }

        

        private static FormaPagamento ObterCodigoPagamento(string forma)
        {
            return forma.ToLower() switch
            {
                "dinheiro" => FormaPagamento.fpDinheiro,
                "cheque" => FormaPagamento.fpCheque,
                "credito" => FormaPagamento.fpCartaoCredito,
                "debito" => FormaPagamento.fpCartaoDebito,
                "pix" => FormaPagamento.fpPagamentoInstantaneoPIXEstatico,
                _ => FormaPagamento.fpOutro // Outros
            };
        }
    }
}
