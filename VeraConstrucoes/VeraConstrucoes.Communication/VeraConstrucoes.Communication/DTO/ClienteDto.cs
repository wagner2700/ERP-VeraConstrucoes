namespace VeraConstrucoes.Communication.DTO
{
    public class ClienteDto

    {
        public string TipoDocumento { get; set; } // "cpf", "cnpj", "anonimo"
        public string Documento { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public EnderecoDto Endereco { get; set; }
    }
}
