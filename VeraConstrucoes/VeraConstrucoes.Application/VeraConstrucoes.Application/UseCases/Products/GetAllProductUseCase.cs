using AutoMapper;
using VeraConstrucoes.Application.UseCases.Products.Interfaces;
using VeraConstrucoes.Communication.DTO;
using VeraConstrucoes.Communication.Response;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Application.UseCases.Products
{
    public class GetAllProductUseCase : IGetAllProductUseCase
    {
        private readonly IProductReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllProductUseCase(IProductReadOnlyRepository repository , IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponseRegisterProduct>> ObterProdutoDescritivoOuCodigo(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return Enumerable.Empty<ResponseRegisterProduct>();

            var produtos = await _repository.BuscarPorCodigoOuDescricao(termo);

            return produtos.Select(p => new ResponseRegisterProduct
            {
                id = p.id,
                descricao = p.descricao,
                valorUnitario = p.valorUnitario,
                estoque = p.estoque,
                Ncm = p.Ncm
            }).ToList();
            
        }

        public async Task<ResponseProductsJson> Execute()
        {
            var result = await _repository.GetAll();

            return new ResponseProductsJson
            {
                Products = _mapper.Map<List<ResponseRegisterProduct>>(result),
            };
        }

        public async Task<PagedResponseProductsJson> ExecutePaged(int page, int pageSize)
        {
            var (items, total) = await _repository.GetAllPaged(page, pageSize);

            return new PagedResponseProductsJson
            {
                Items = _mapper.Map<IEnumerable<ProdutoDto>>(items),
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }
    }
}
