using VeraConstrucoes.Communication.Response;

namespace VeraConstrucoes.Application.UseCases.Products.Interfaces
{
    public interface IGetAllProductUseCase
    {
        Task<ResponseProductsJson> Execute();
        Task<IEnumerable<ResponseRegisterProduct>> ObterProdutoDescritivoOuCodigo(string termo);
        Task<PagedResponseProductsJson> ExecutePaged(int page, int pageSize);

    }
}
