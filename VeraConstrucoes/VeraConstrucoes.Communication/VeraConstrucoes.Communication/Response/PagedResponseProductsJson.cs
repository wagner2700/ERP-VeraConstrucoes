using VeraConstrucoes.Communication.DTO;

namespace VeraConstrucoes.Communication.Response
{
    public class PagedResponseProductsJson
    {
        public IEnumerable<ProdutoDto> Items { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}
