namespace Poc.Order.Processor.Domain.Interfaces
{
    public interface IPedidoSubscriber
    {
        Task ConsumePedido(CancellationToken cancellationToken);
    }
}