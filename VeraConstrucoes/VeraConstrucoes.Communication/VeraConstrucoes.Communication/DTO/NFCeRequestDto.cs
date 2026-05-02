namespace VeraConstrucoes.Communication.DTO
{
    public class NFCeRequestDto
    {
        public int Numero {  get; set; }
        public int Serie { get; set; }
        public ClienteDto Cliente { get; set; }

        public string Observacoes { get; set; }
        public decimal Desconto { get; set; }
        public decimal Acrescimo { get; set; }


        public List<ProdutoDto> Produtos { get; set; }
        public List<PagamentoDto> Pagamentos { get; set; }

    }
}
