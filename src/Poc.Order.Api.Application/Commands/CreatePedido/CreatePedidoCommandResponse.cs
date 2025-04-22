using Poc.Order.Api.Domain.Enums;

namespace Poc.Order.Api.Application.Commands.CreatePedido
{
    public class CreatePedidoCommandResponse
    {
        public int PedidoId { get; set; }

        public StatusPedido Status { get; set; }
    }
}
