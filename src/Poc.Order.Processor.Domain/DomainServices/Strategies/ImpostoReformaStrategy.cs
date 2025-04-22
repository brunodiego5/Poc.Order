using Poc.Order.Processor.Domain.Entities;
using Poc.Order.Processor.Domain.Interfaces;

namespace Poc.Order.Processor.Domain.DomainServices.Strategies
{

    public class ImpostoReformaStrategy : IPedidoImpostoStrategy
    {
        public string Nome => "Reforma";

        public decimal CalcularImposto(decimal totalItens) => totalItens * 0.20m;
    }
}