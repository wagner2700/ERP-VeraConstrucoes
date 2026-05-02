namespace VeraConstrucoes.Communication.Request
{
    public class CancelamenoNotaFiscalRequest
    {
        public int idNota {  get; set; }
        public string protocoloAutorizacao {  get; set; } = string.Empty;
        public string chaveNfe {  get; set; } = string.Empty;
    }
}
