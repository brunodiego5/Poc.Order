﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poc.Order.Api.Application.Commands.CreatePedido;
using Poc.Order.Api.Application.Commands.UpdateImpostoPedido;
using Poc.Order.Api.Application.Queries.GetOnePedido;
using Poc.Order.Api.Application.Queries.GetPedidos;
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

        [HttpGet("{pedidoId:int}")]
        public async Task<IActionResult> GetOne(int pedidoId)
        {
            var result = await mediator.Send(new GetOnePedidoQuery { PedidoId = pedidoId });

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] StatusPedido status)
        {
            var result = await mediator.Send(new GetPedidosQuery { Status = status });

            if (result is null || result?.Count == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateImposto([FromBody] UpdateImpostoPedidoCommand command)
        {
            var result = await mediator.Send(command);

            if (result?.Status == StatusPedido.Cancelado)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
