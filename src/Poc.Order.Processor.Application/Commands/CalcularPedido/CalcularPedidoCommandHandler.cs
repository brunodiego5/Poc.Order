using MediatR;
using Microsoft.Extensions.Logging;
using Poc.Order.Processor.Domain.Interfaces;

namespace Poc.Order.Processor.Application.Commands.CalcularPedido
{
    public class CalcularPedidoCommandHandler : IRequestHandler<CalcularPedidoCommand, bool>
    {
        private readonly IPedidoDomainService pedidoDomainService;

        private readonly ILogger<CalcularPedidoCommandHandler> logger;

        public CalcularPedidoCommandHandler(IPedidoDomainService pedidoDomainService, ILogger<CalcularPedidoCommandHandler> logger)
        {
            this.pedidoDomainService = pedidoDomainService;
            this.logger = logger;
        }

        public async Task<bool> Handle(CalcularPedidoCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Calculando o imposto do pedido. CorrelationId {request?.CorrelationId}");
            try
            {
                var valorImposto = pedidoDomainService.CalcularImposto(request?.Itens?.Sum(s => s?.Quantidade * s?.Valor) ?? 0);

                logger.LogDebug($"Valor do imposto {valorImposto:0.00}. CorrelationId {request?.CorrelationId}");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error ao calcular o pedido. CorrelationId {request?.CorrelationId}.");

                return await Task.FromResult<bool>(false);
            }

            return await Task.FromResult<bool>(true);
        }
    }
}