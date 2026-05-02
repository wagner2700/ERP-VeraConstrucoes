using DFe.Classes.Flags;
using NFCVeraConstrucoes.Helpers;
using NFCVeraConstrucoes.Models.NFCe;
using NFCVeraConstrucoes.Services;
using NFe.Classes;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Informacoes.Pagamento;
using NFe.Classes.Informacoes.Transporte;
using VeraConstrucoes.Infrastructure.Repositories;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;
using static NFCVeraConstrucoes.Models.NFCe.FormaPagamentoNFCe;

namespace NFCVeraConstrucoes.Builders
{
    public class NFCeBuilder
    {

        private readonly ConfiguracaoNF _configuracao;
        private IdentificacaoNFCe _identificacao;
        private EmitenteNFCe _emitente;
        private DestinatarioNFCe _destinatario;
        private readonly List<ProdutoNFCe> _produtos = new List<ProdutoNFCe>();
        private PagamentoNFCe _pagamento;
        private TransporteNFCe _transporte = new TransporteNFCe();
        private readonly IConfiguracaoRepository _configuracaoRepository;
        

        public NFCeBuilder(ConfiguracaoNF configuracao, IConfiguracaoRepository configuracaoRepository)
        {
            _configuracao = configuracao ?? throw new ArgumentNullException(nameof(configuracao));
            _configuracaoRepository = configuracaoRepository;
            InicializarEmitentePadrao();
        }

        private void InicializarEmitentePadrao()
        {
            _emitente = new EmitenteNFCe
            {
                CNPJ = _configuracao.CnpjEmitente,
                RazaoSocial = _configuracao.RazaoSocial,
                NomeFantasia = _configuracao.NomeFantasia,
                InscricaoEstadual = _configuracao.InscricaoEstadual,
                RegimeTributario = _configuracao.RegimeTributario,
                Logradouro = _configuracao.LogradouroEmitente,
                Numero = _configuracao.NumeroEmitente,
                Bairro = _configuracao.BairroEmitente,
                //Complemento = _configuracao.ComplementoEmitente,
                CodigoMunicipioIBGE = _configuracao.CodigoMunicipioIBGE,
                NomeMunicipio = _configuracao.NomeMunicipio,
                UF = _configuracao.UF,
                CEP = _configuracao.CEP,
                
                //Telefone = _configuracao.Telefone
            };
        }

        public NFCeBuilder ComIdentificacao(int numeroNota, string naturezaOperacao = "VENDA AO CONSUMIDOR")
        {
            _identificacao = new IdentificacaoNFCe
            {
                UF = DFe.Classes.Entidades.Estado.SP,
                Ambiente = TipoAmbiente.Homologacao,
                NumeroNota = numeroNota
            };
            return this;
        }

        public NFCeBuilder ComDestinatario(string cpf = null, string cnpj = null, string nome = null)
        {
            _destinatario = string.IsNullOrEmpty(cpf) && string.IsNullOrEmpty(cnpj)
                ? DestinatarioNFCe.Destinatario()
                : new DestinatarioNFCe
                {
                    CPF = cpf,
                    CNPJ = cnpj,
                    Nome = nome ?? "CONSUMIDOR FINAL"
                };
            return this;
        }

        public NFCeBuilder AdicionarProduto(ProdutoNFCe produto)
        {
            produto.NumeroItem = _produtos.Count + 1;
            _produtos.Add(produto);
            return this;
        }

        public NFCeBuilder AdicionarProduto(
            string codigo,
            string descricao,
            string ncm,
            int cfop,
            string unidade,
            decimal quantidade,
            decimal valorUnitario,
            decimal aliquotaICMS = 18.00m,
            string cst = "00")
        {
            var produto = new ProdutoNFCe
            {
                Codigo = codigo,
                Descricao = descricao,
                NCM = ncm,
                CFOP = cfop,
                Unidade = unidade,
                Quantidade = quantidade,
                ValorUnitario = valorUnitario,
                AliquotaICMS = aliquotaICMS,
                CST = cst
                
            };
            return AdicionarProduto(produto);
        }

        public NFCeBuilder ComPagamento(List<FormaPagamentoNFCe> formasPagamento)
        {
            _pagamento = new PagamentoNFCe();
            foreach (var forma in formasPagamento)
            {
                _pagamento.FormasPagamento.Add(forma);
            }
            return this;
        }

        public NFCeBuilder ComPagamentoSimples(decimal valorTotal, FormaPagamento tipoPagamento = FormaPagamento.fpDinheiro)
        {
            _pagamento = new PagamentoNFCe();
            _pagamento.FormasPagamento.Add(new FormaPagamentoNFCe
            {
                Tipo = tipoPagamento,
                Valor = valorTotal,
                Indicador = IndicadorPagamentoDetalhePagamento.ipDetPgVista
            });
            return this;
        }

        //public NFCeBuilder Transporte()
        //{
        //    _transporte = new TransporteNFCe();
           
        //    return this;

        //}

        public NFe.Classes.NFe Construir()
        {
            ValidarCamposObrigatorios();


            // Calcular totais
            var total = TotalNFCe.CalcularTotais(_produtos);
            
            // Converter para modelos Zeus
            var ide = _identificacao.ToZeusModel(ConfigHelper.GerarCNF());
            var emit = _emitente.ToZeusModel();
            var dest = _destinatario.ToZeusModel(VersaoServico.Versao400);
            var pag = _pagamento.ToZeusModel();
            var tot = total.ToZeusModel();
            var transp = _transporte.ToZeusModel();
            

            // Converter produtos
            var detList = new List<NFe.Classes.Informacoes.Detalhe.det>();
            foreach (var produto in _produtos.OrderBy(p => p.NumeroItem))
            {
                detList.Add(produto.ToZeusModel());
            }

            // Formatar cada componente separadamente
            string cUFFormatado = ide.cUF.ToString().PadLeft(2, '0');
            string dataFormatada = ide.dhEmi.ToString("yyMM");
            string modFormatado = ide.mod.ToString().PadLeft(2, '0');
            string serieFormatada = ide.serie.ToString().PadLeft(3, '0');
            string nNFFormatado = ide.nNF.ToString().PadLeft(9, '0');

            // Construir NFCe
            return new NFe.Classes.NFe
            {
                infNFe = new infNFe
                {
                    versao = "4.00",
                    ide = ide,
                    emit = emit,
                    dest = dest,
                    det = detList,
                    pag = new List<pag> { pag },
                    transp = transp,
                    total = tot,
                    
                }
            };
        }

        private void ValidarCamposObrigatorios()
        {
            if (_identificacao == null)
                throw new InvalidOperationException("Identificação não configurada");

            if (_emitente == null)
                throw new InvalidOperationException("Emitente não configurado");

            if (_destinatario == null)
                _destinatario = DestinatarioNFCe.Destinatario();

            if (_produtos.Count == 0)
                throw new InvalidOperationException("Nenhum produto adicionado");

            if (_pagamento == null)
                throw new InvalidOperationException("Pagamento não configurado");
        }

        //public static NFe.Classes.NFe CriarExemplo(ConfiguracaoNFCe configuracao, int numeroNota)
        //{
        //    var builder = new NFCeBuilder(configuracao);

        //    builder.ComIdentificacao(numeroNota)
        //           .ComDestinatario("12345678909", null, "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL")
        //           .AdicionarProduto(
        //               codigo: "001",
        //               descricao: "NOTA FISCAL EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
        //               ncm: "96170010",
        //               cfop: 5102,
        //               unidade: "UN",
        //               quantidade: 1,
        //               valorUnitario: 10.00m,
        //               aliquotaICMS: 0m)
        //           .AdicionarProduto(
        //               codigo: "002",
        //               descricao: "NOTA FISCAL EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
        //               ncm: "96170010",
        //               cfop: 5102,
        //               unidade: "UN",
        //               quantidade: 1,
        //               valorUnitario: 10.00m,
        //               aliquotaICMS: 0m)
        //           .ComPagamentoSimples(20.00m);

        //    return builder.Construir();
        //}
    }
}
