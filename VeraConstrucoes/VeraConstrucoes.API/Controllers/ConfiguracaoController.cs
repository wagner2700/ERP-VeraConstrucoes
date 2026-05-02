using Microsoft.AspNetCore.Mvc;
using VeraConstrucoes.Application.UseCases.Configuracoes.Interface;
using VeraConstrucoes.Domain.Entities.Configuracoes;

namespace VeraConstrucoes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracaoController : ControllerBase
    {
        private readonly IGerenciarConfiguracaoUseCase _useCase;

        public ConfiguracaoController(IGerenciarConfiguracaoUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet]
        public async Task<IActionResult> Obter()
        {
            var config = await _useCase.ObterConfiguracao();
            if (config == null)
                return NoContent() ;
            return Ok(config);
        }

        [HttpPost]
        public async Task<IActionResult> Salvar([FromBody] ConfiguracaoNFCe config)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var resultado = await _useCase.SalvarConfiguracao(config);
                return Ok(resultado);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { erro = $"Erro ao salvar configuração: {ex.Message}" });
            }
        }
    }
}
