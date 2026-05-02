using NFe.Classes.Informacoes.Pagamento;

namespace NFCVeraConstrucoes.Enums
{
    public static class TipoEnumeradores
    {

        // Formas de pagamento comuns para NFCe
        public static readonly FormaPagamento[] FormasPagamentoComuns =
        {
            FormaPagamento.fpDinheiro,
            FormaPagamento.fpCartaoCredito,
            FormaPagamento.fpCartaoDebito,
            FormaPagamento.fpCreditoEmLoja,
            FormaPagamento.fpValeAlimentacao,
            FormaPagamento.fpValeRefeicao,
            FormaPagamento.fpValePresente,
            FormaPagamento.fpPagamentoInstantaneoPIXDinamico
        };

        // CSTs ICMS mais comuns
        public static class CSTs
        {
            public const string TributadaIntegralmente = "00";
            public const string TributadaComReducaoBC = "20";
            public const string Isenta = "40";
            public const string NaoTributada = "41";
            public const string Suspensao = "51";
        }

        // CFOPs mais comuns para NFCe
        public static class CFOPs
        {
            public const int VendaConsumidorEstado = 5102;
            public const int VendaConsumidorOutroEstado = 6102;
            public const int PrestacaoServico = 5933;
            public const int VendaCombustivel = 5656;
        }
    }
}
