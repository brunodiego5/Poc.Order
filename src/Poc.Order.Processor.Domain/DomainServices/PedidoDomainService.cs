using Microsoft.FeatureManagement;
using Poc.Order.Processor.Domain.Interfaces;

namespace Poc.Order.Processor.Domain.DomainServices
{

    public class PedidoDomainService : IPedidoDomainService
    {
        private readonly IEnumerable<IPedidoImpostoStrategy> strategies;
        private readonly IFeatureManager flag;

        public PedidoDomainService(IEnumerable<IPedidoImpostoStrategy> strategies, IFeatureManager flag)
        {
            this.strategies = strategies;
            this.flag = flag;
        }

        public async Task<decimal> CalcularImposto(decimal totalItens)
        {
            var nome = await flag.IsEnabledAsync("ReformaTributaria")
                ? "Reforma" 
                : "Atual";

            var strategy = strategies.First(w => w.Nome == nome);

            return strategy.CalcularImposto(totalItens);
        }
    }
}