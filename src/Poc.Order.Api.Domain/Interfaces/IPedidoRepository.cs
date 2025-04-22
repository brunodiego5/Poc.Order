using Poc.Order.Api.Domain.Entities;
using Poc.Order.Api.Domain.Enums;

namespace Poc.Order.Api.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task AddPedidoAsync(Pedido pedido, CancellationToken cancellationToken);

        Task<Pedido> GetPedidoByIdAsync(int pedidoId, CancellationToken cancellationToken);

        Task<IList<Pedido>> GetPedidosByStatusAsync(StatusPedido statusPedido, CancellationToken cancellationToken);

        Task UpdateImpostoPedidoAsync(int pedidoId, decimal imposto, CancellationToken cancellationToken);
    }
}