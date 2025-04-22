using Poc.Order.Api.Domain.Enums;

namespace Poc.Order.Api.Application.Commands.UpdateImpostoPedido
{
    public class UpdateImpostoPedidoCommandResponse
    {
        public int PedidoId { get; set; }

        public StatusPedido Status { get; set; }
    }
}
