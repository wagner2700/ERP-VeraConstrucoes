using VeraConstrucoes.Application.UseCases.NFC.Interface;
using VeraConstrucoes.Communication.DTO;
using VeraConstrucoes.Domain.NFCe;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Application.UseCases.NFC
{
    public class ReadOnlyNotaFiscalUseCase : IReadOnlyNotaFiscalUseCase
    {
        private readonly INFCRepository _nfcRepository;
        public ReadOnlyNotaFiscalUseCase(INFCRepository nFCRepository)
        {
            _nfcRepository = nFCRepository;
        }


        

        public async Task<NfcDetalhesDTO> LerDetalhesNotaFiscal(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido");

            var detalhes = await _nfcRepository.ObterDetalhesCompletoAsync(id);
            if (detalhes == null)
                throw new KeyNotFoundException("Nota fiscal não encontrada");

            return detalhes;
        }

        public async Task<List<NFCeResumoDTO>> ListarNotasFiscais()
        {
            var lista = await _nfcRepository.ObterNotasFiscais();
            return lista;
        }
    }
}
