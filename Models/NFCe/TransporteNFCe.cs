using NFe.Classes.Informacoes.Transporte;

namespace NFCVeraConstrucoes.Models.NFCe
{
    public class TransporteNFCe
    {
        


        public transp ToZeusModel()
        {
            return new transp { modFrete = ModalidadeFrete.mfSemFrete };
        }
    }
}
