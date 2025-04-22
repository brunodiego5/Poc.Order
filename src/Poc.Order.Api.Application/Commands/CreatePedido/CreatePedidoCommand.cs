using MediatR;

namespace Poc.Order.Api.Application.Commands.CreatePedido
{
    public class CreatePedidoCommand : IRequest<CreatePedidoCommandResponse>
    {
        public int PedidoId { get; set; }

        public int ClientId { get; set; }

        public required IList<ItemPedidoCommand> Itens { get; set; }
    }

    public record ItemPedidoCommand(int ProdutoId, decimal Quantidade, decimal Valor);
}
