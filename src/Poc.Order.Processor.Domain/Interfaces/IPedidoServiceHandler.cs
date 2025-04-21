using Poc.Order.Processor.Domain.Entities;

namespace Poc.Order.Processor.Domain.Interfaces
{
    public interface IPedidoServiceHandler
    {
        Task<bool> EnviarImposto(Pedido pedido, string correlationId, CancellationToken cancellationToken);
    }
}
