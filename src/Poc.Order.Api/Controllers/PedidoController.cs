using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poc.Order.Api.Application.Commands.CreatePedido;

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

            return Ok(result);
        }
    }
}
