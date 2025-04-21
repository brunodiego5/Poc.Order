using MediatR;

namespace Poc.Order.Api.Application.Commands.UpdateImpostoPedido
{
    public class UpdateImpostoPedidoCommand : IRequest<Unit>
    {
        public string CorrelationId { get; set; }

        public int PedidoId { get; set; }

        public decimal Imposto { get; set; }
    }
}
