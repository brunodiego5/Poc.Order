using Poc.Order.Processor.Domain.Entities;
using Poc.Order.Processor.Domain.Interfaces;

namespace Poc.Order.Processor.Domain.DomainServices.Strategies
{

    public class ImpostoAtualStrategy : IPedidoImpostoStrategy
    {
        public string Nome => "Atual";

        public decimal CalcularImposto(decimal totalItens) => totalItens * 0.30m;
    }
}