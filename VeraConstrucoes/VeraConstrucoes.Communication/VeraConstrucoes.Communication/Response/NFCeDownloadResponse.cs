namespace VeraConstrucoes.Communication.Response
{
    public class NFCeDownloadResponse
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public int? NFCeId { get; set; } // Id da nota no banco (se criada/atualizada)
    }
}
