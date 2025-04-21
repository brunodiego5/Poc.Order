using MediatR;

namespace Poc.Order.Processor.Application.Commands.CalcularPedido
{
    public class CalcularPedidoCommand : IRequest<bool>
    {
        public string CorrelationId { get; set; }

        public int PedidoId { get; set; }

        public required IList<ItemPedidoCommand> Itens { get; set; }
    }

    public record ItemPedidoCommand(decimal Quantidade, decimal Valor);
}
