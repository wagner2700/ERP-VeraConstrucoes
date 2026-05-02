using VeraConstrucoes.Application.UseCases.Products.Interfaces;
using VeraConstrucoes.Communication.Request;
using VeraConstrucoes.Communication.Response;
using VeraConstrucoes.Domain.Entities.Produtos;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Application.UseCases.Products
{
    public class RegisterProductUseCase : IRegisterProductUseCase
    {
        private readonly IProductWriteOnlyRepository _productWriteOnlyRepository;

        public RegisterProductUseCase(IProductWriteOnlyRepository productWriteOnlyRepository)
        {
            _productWriteOnlyRepository = productWriteOnlyRepository;
        }

        public async Task<ResponseRegisterProduct> Execute(RequestRegisterProduct request)
        {
            var produto = new Produto
            {
                descricao = request.descricao,
                estoque = request.estoque,
                valorUnitario = request.valorUnitario,
                Ncm = string.IsNullOrWhiteSpace(request.ncm) ? null : request.ncm
            };
            await _productWriteOnlyRepository.Add(produto);

            return new ResponseRegisterProduct
            {
                id = produto.id,
                descricao = request.descricao,
                valorUnitario  = request.valorUnitario,
                estoque = request.estoque,
                Ncm = produto.Ncm ?? string.Empty
            };
        }
    }
}
