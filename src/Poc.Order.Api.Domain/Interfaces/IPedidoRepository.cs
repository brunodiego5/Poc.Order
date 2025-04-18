using Poc.Order.Api.Domain.Entities;

namespace Poc.Order.Api.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task AddPedidoAsync(Pedido pedido, CancellationToken cancellationToken);

        Task<Pedido> GetPedidoByIdAsync(int id, CancellationToken cancellationToken);
    }
}