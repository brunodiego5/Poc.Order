using Poc.Order.Api.Domain.Entities;

namespace Poc.Order.Api.Domain.Interfaces
{
    public interface IPedidoPublisher
    {
        Task PublishPedido(Pedido pedido, string correlationId, CancellationToken cancellationToken);
    }
}