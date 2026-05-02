using NFCVeraConstrucoes.Models;
using NFCVeraConstrucoes.Models.NFCe;
using VeraConstrucoes.Application.UseCases.NFC.Interface;
using VeraConstrucoes.Communication.DTO;
using VeraConstrucoes.Domain.Entities.NFCe;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;
using static NFCVeraConstrucoes.Models.NFCe.FormaPagamentoNFCe;

namespace VeraConstrucoes.Application.UseCases.NFC
{
    public class SaveNFCUseCase : ISaveNFCUseCase
    {
        private readonly INFCRepository _nFCRepository;


        public SaveNFCUseCase(INFCRepository nFCRepository)
        {
            _nFCRepository = nFCRepository;
        }


        public async Task<Domain.Entities.NFCe.NFC> GravarDadosIniciaisNotaFiscal(NFCeRequestDto request)
        {
            // 2. Criar a entidade NFCe principal
            var nfce = new Domain.Entities.NFCe.NFC
            {
                Numero = request.Numero,
                Serie = request.Serie,
                DataEmissao = DateTime.Now,
                ValorTotal = request.Produtos.Sum(p => p.ValorTotal),
                SituacaoNota = "L",
                StatusProcessamento = false,
                ClienteDocumento = request.Cliente?.Documento ?? string.Empty,
                ClienteNome = request.Cliente?.Nome ?? string.Empty,
                ClienteEmail = request.Cliente?.Email ?? string.Empty,
                ClienteTelefone = request.Cliente?.Telefone ?? string.Empty
            };

            // 4. Adicionar os produtos
            nfce.Produtos = request.Produtos.Select(p => new ProdutoNF
            {
                //Codigo = p.id ?? string.Empty,
                Descricao = p.Descricao ?? string.Empty,
                Quantidade = p.Quantidade,
                ValorUnitario = p.ValorUnitario,
                // ValorTotal é calculado automaticamente (propriedade somente leitura)
                Ncm = p.NCM ?? string.Empty,
                Cfop = p.CFOP,
                Unidade = p.Unidade ?? string.Empty,
                //Cest = p.ce ?? string.Empty
            }).ToList();

            // 5. Adicionar os pagamentos
            nfce.Pagamentos = request.Pagamentos.Select(p => new PagamentoNF
            {
                Metodo = p.metodo ?? string.Empty,
                Valor = p.Valor,
                Parcelas = 0,
                Troco = 0,

            }).ToList();

            return await _nFCRepository.AddNFC(nfce);

        }

        public async Task<ResultadoEmissao> Execute(NFCeRequestDto request,  ResultadoEmissao? resultado = null)
        {
            // 2. Criar a entidade NFCe principal
            var nfce = new Domain.Entities.NFCe.NFC
            {
                Id = resultado.Id,
                Numero = (int)resultado.Numero,
                Serie = resultado.Serie,
                DataEmissao = resultado.DataHora,
                ValorTotal = request.Produtos.Sum(p => p.ValorTotal),
                StatusProcessamento = true,
                SituacaoNota = "F",
                ClienteDocumento = request.Cliente?.Documento ?? string.Empty,
                ClienteNome = request.Cliente?.Nome ?? string.Empty,
                ClienteEmail = request.Cliente?.Email ?? string.Empty,
                ClienteTelefone = request.Cliente?.Telefone ?? string.Empty
            };
            // 3. Adicionar as propriedades complementares (NFCePropriedades)
            nfce.Complemento = new NFCePropriedades
            {
                Observacoes = request.Observacoes ?? string.Empty,
                Desconto = request.Desconto,
                Acrescimo = request.Acrescimo,
                XmlPath = resultado.XmlPath ?? string.Empty,
                PdfPath = resultado.PdfPath ?? string.Empty,
                QrCodeUrl = resultado.QrCodeUrl ?? string.Empty,
                MensagemErro = resultado.MensagemErro ?? string.Empty,
                ChaveAcesso = resultado.ChaveAcesso ?? string.Empty,
                Protocolo = resultado.Protocolo ?? string.Empty
            };

            // 4. Adicionar os produtos
            nfce.Produtos = request.Produtos.Select(p => new ProdutoNF
            {
                //Codigo = p.id ?? string.Empty,
                Descricao = p.Descricao ?? string.Empty,
                Quantidade = p.Quantidade,
                ValorUnitario = p.ValorUnitario,
                // ValorTotal é calculado automaticamente (propriedade somente leitura)
                Ncm = p.NCM ?? string.Empty,
                Cfop = p.CFOP ,
                Unidade = p.Unidade ?? string.Empty,
                //Cest = p.ce ?? string.Empty
            }).ToList();

            // 5. Adicionar os pagamentos
            nfce.Pagamentos = request.Pagamentos.Select(p => new PagamentoNF
            {
                Metodo = p.metodo ?? string.Empty,
                Valor = p.Valor,
                Parcelas =0,
                Troco = 0,
               
            }).ToList();

            var result = await _nFCRepository.AddNFC(nfce);
            resultado.Id = result.Id;

            return resultado;
        }
    }
}
