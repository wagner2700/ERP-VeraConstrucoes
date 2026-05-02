using CashFlow.Communication.Responses;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using VeraConstrucoes.Application.UseCases.Products.Interfaces;
using VeraConstrucoes.Communication.Request;
using VeraConstrucoes.Communication.Response;

namespace VeraConstrucoes.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProdutoController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterProduct),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromServices] IRegisterProductUseCase useCase  , [FromBody] RequestRegisterProduct request)
        {
            

            var response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseProductsJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllProduct([FromServices] IGetAllProductUseCase useCase)
        {
            

            var response = await useCase.Execute();
            if (response.Products?.Count != 0 && response != null)
                return Ok(response.Products);

            return NoContent();
        }

        [HttpGet("paginado")]
        [ProducesResponseType(typeof(PagedResponseProductsJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProductPaged(
            [FromServices] IGetAllProductUseCase useCase,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = await useCase.ExecutePaged(page, pageSize);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduto([FromServices] IWriteProductUseCase useCase , [FromBody] RequestUpdateProductJson request)
        {
            await useCase.UpdateProduto(request);
            return Ok();
        }

        /// <summary>
        /// Busca produtos por ID ou descrição.
        /// </summary>
        /// <param name="id">ID do produto (opcional)</param>
        /// <param name="descricao">Descrição do produto (opcional)</param>
        /// <returns>Lista de produtos encontrados</returns>
        [HttpPost("buscar")]
        public async Task<ActionResult<List<ResponseRegisterProduct>>> BuscarProdutos([FromBody] string termoBusca , [FromServices] IGetAllProductUseCase useCase)
        {
            try
            {
                var resultado = await useCase.ObterProdutoDescritivoOuCodigo(termoBusca);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = $"Erro interno: {ex.Message}" });
            }
        }

    }
}
