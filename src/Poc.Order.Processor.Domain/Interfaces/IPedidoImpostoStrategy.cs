using Poc.Order.Processor.Domain.Entities;

namespace Poc.Order.Processor.Domain.Interfaces
{
    public interface IPedidoImpostoStrategy
    {
        string Nome { get; }
        decimal CalcularImposto(decimal totalItens);
    }
}