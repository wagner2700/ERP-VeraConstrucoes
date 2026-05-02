using Microsoft.AspNetCore.Mvc;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NcmController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromServices] INCMRepository repository,
            [FromQuery] string? termo = null)
        {
            var lista = await repository.ListarTodosAsync();

            if (!string.IsNullOrWhiteSpace(termo))
            {
                var filtro = termo.Trim().ToLowerInvariant();

                lista = lista
                    .Where(item =>
                        item.Ncm.ToLower().Contains(filtro) ||
                        (item.descricao ?? string.Empty).ToLower().Contains(filtro) ||
                        item.csosn.ToString().Contains(filtro))
                    .ToList();
            }

            return Ok(lista.Select(item => new
            {
                ncm = item.Ncm,
                descricao = item.descricao ?? string.Empty,
                csosn = item.csosn,
            }));
        }
    }
}
