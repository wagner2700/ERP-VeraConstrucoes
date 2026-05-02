using AutoMapper;
using VeraConstrucoes.Application.UseCases.Products.Interfaces;
using VeraConstrucoes.Communication.Request;
using VeraConstrucoes.Domain.Entities.Produtos;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Application.UseCases.Products
{
    public class WriteProductUseCase : IWriteProductUseCase
    {
        private readonly IMapper _mapper;
        private readonly IProductWriteOnlyRepository _productWriteOnlyRepository;
        public WriteProductUseCase(IMapper mapper, IProductWriteOnlyRepository productWriteOnlyRepository)
        {
            _mapper = mapper;
            _productWriteOnlyRepository = productWriteOnlyRepository;
        }

        public async Task UpdateProduto(RequestUpdateProductJson request)
        {
            var produto = _mapper.Map<Produto>(request);
            if (produto != null)
            {
                await _productWriteOnlyRepository.Update(produto);
            }
        }
    }
}
