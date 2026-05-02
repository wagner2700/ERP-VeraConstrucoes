using DFe.Classes.Flags;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Informacoes.Pagamento;

namespace NFCVeraConstrucoes.Models.NFCe
{
    public class FormaPagamentoNFCe
    {
        public FormaPagamento Tipo { get; set; }
        public decimal Valor { get; set; }
        public IndicadorPagamentoDetalhePagamento Indicador { get; set; }

        public detPag ToZeusModel()
        {
            return new detPag
            {
                tPag = Tipo,
                vPag = Valor,
                indPag = Indicador

            };
        }

        public class PagamentoNFCe
        {
            public List<FormaPagamentoNFCe> FormasPagamento { get; set; } = new List<FormaPagamentoNFCe>();

            public pag ToZeusModel()
            {
                var detPagList = new List<detPag>();

                foreach (var forma in FormasPagamento)
                {
                    detPagList.Add(forma.ToZeusModel());
                }

                
                return new pag()
                {
                    detPag = detPagList
                };
            }

            public void AdicionarPagamento(FormaPagamento tipo, decimal valor,
                IndicadorPagamentoDetalhePagamento indicador)
            {
                FormasPagamento.Add(new FormaPagamentoNFCe
                {
                    Tipo = tipo,
                    Valor = valor,
                    Indicador = indicador
                });
            }
        }
    }
 }
