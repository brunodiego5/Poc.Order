using Poc.Order.Processor.Domain.Entities;

namespace Poc.Order.Processor.Domain.Interfaces
{
    public interface IPedidoDomainService
    {
        Task<decimal> CalcularImposto(decimal totalItens);
    }
}