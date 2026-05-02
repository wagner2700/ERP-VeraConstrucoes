using Microsoft.AspNetCore.Mvc;
using NFCVeraConstrucoes.Helpers;
using NFCVeraConstrucoes.Models;
using NFCVeraConstrucoes.Services;
using NFe.Servicos.Retorno;
using System.Security.Cryptography.X509Certificates;
using VeraConstrucoes.Application.UseCases.NFC.Interface;
using VeraConstrucoes.Communication.DTO;
using VeraConstrucoes.Communication.Request;

namespace VeraConstrucoes.API.Controllers
{
    [Route("api/[controller]")]
    public class NfcController : ControllerBase
    {

        [HttpPost("transmitir")]
        [ProducesResponseType(typeof(ResultadoEmissao), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StreamNFC([FromBody] NFCeRequestDto request, [FromServices] IEmitNFCUseCase useCase, [FromServices] ISaveNFCUseCase useCaseSave)
        {

            var response = await useCase.Execute(request);
            if (response.Sucesso)
            {
                await useCaseSave.Execute(request, response);
            }

            return Ok(response);
        }


        //[HttpPost("SalvarNFC")]
        //public async Task<IActionResult> SalvarNFC([FromBody] NFCeRequestDto request, ISaveNFCUseCase useCase)
        //{
        //    var response = await useCase.Execute(request );
        //    return Ok(response);
        //}

        [HttpPost("downloadXmlNf")]
        public async Task DownloadXml([FromBody] string chave ,[FromServices] IEmitNFCUseCase useCase)
        {
            await useCase.DownloadXml(chave);
        }


        [HttpPost("cancelar")]
        public async Task<IActionResult> CancelamentoNFCe([FromBody] CancelamenoNotaFiscalRequest requestCancelamento, [FromServices] IEmitNFCUseCase useCase)
        {
            RetornoRecepcaoEvento retornoEvento =  await useCase.CancelamentoNFCe(requestCancelamento);

            return Ok(retornoEvento.Retorno.retEvento);
        }


        [HttpGet]
        public async Task StatusServicoSefaz([FromServices] ISefazServiceUseCase useCase)
        {
            await useCase.CheckStatusAsync();
        }

        [HttpGet("listarNotasFiscais")]
        public async Task<IActionResult> GetALLNf([FromServices] IReadOnlyNotaFiscalUseCase useCase)
        {
            var lista = await useCase.ListarNotasFiscais();
            return Ok(lista);
        }



        [HttpGet("detalhes/{id}")]
        public async Task<IActionResult> ObterDetalhesNotaFiscal(int id, [FromServices] IReadOnlyNotaFiscalUseCase useCase)
        {
            try
            {
                var result = await useCase.LerDetalhesNotaFiscal(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                // Log do erro interno (ex: usando ILogger)
                return StatusCode(500, new { erro = "Erro interno ao processar a solicitação" });
            }
        }


    }
}
