using MediatR;

namespace Poc.Order.Api.Application.Commands.UpdateImpostoPedido
{
    public class UpdateImpostoPedidoCommand : IRequest<Unit>
    {
        public int PedidoId { get; set; }

        public double Imposto { get; set; }
    }
}
