

using NFCVeraConstrucoes.Models.NFCe;
using VeraConstrucoes.Domain.Entities.NFCe;

namespace VeraConstrucoes.Domain.NFCe
{
    public class NFCeCompleta
    {
        public NfcModel NFC {  get; set; }
        public NFCePropriedades NFCePropriedades { get; set; }
        public ProdutoNFCe ProdutoNFCe { get; set; }
        public FormaPagamentoNFCe FormaPagamentoNFCe { get;  set; }
    }
}
