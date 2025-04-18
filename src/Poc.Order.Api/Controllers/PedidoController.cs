using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poc.Order.Api.Application.Commands.CreatePedido;
using Poc.Order.Api.Application.Queries.GetOnePedido;
using Poc.Order.Api.Domain.Enums;

namespace Poc.Order.Api.Controllers
{
    [ApiController]
    [Route("api/pedido")]
    public class PedidoController : Controller
    {
        private readonly IMediator mediator;

        public PedidoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePedidoCommand command)
        {
            var result = await mediator.Send(command);

            if (result?.Status == StatusPedido.Erro)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var result = await mediator.Send(new GetOnePedidoQuery { PedidoId = id});

            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}
