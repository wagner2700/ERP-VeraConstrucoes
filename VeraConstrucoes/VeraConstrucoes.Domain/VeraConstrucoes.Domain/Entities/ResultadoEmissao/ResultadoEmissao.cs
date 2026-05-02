namespace NFCVeraConstrucoes.Models
{
    public class ResultadoEmissao
    {
        public int Id { get; set; }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string MensagemErro { get; set; }

        // Dados da NFC-e
        public string ChaveAcesso { get; set; }
        public string Protocolo { get; set; }
        public long Numero { get; set; }
        public int Serie { get; set; }

        // Arquivos gerados
        public string XmlPath { get; set; }
        public string PdfPath { get; set; }
        public string QrCodeUrl { get; set; }

        // Data/hora
        public DateTime DataHora { get; set; } = DateTime.Now;

        // Método auxiliar para facilitar o uso
        public static ResultadoEmissao Erro(string mensagem)
        {
            return new ResultadoEmissao
            {
                Sucesso = false,
                MensagemErro = mensagem
            };
        }

        public static ResultadoEmissao SucessoCompleto(
            string chave, string protocolo, int numero, int serie)
        {
            return new ResultadoEmissao
            {
                Sucesso = true,
                Mensagem = "NFC-e emitida com sucesso!",
                ChaveAcesso = chave,
                Protocolo = protocolo,
                Numero = numero,
                Serie = serie
            };
        }
    }
}
