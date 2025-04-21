using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Poc.Order.Processor.Domain.Entities;
using Poc.Order.Processor.Domain.Interfaces;

namespace Poc.Order.Processor.Application.Commands.CalcularPedido
{
    public class CalcularPedidoCommandHandler : IRequestHandler<CalcularPedidoCommand, bool>
    {
        private readonly IPedidoDomainService pedidoDomainService;

        private readonly IPedidoServiceHandler pedidoServiceHandler;

        private readonly IMapper mapper;

        private readonly ILogger<CalcularPedidoCommandHandler> logger;

        public CalcularPedidoCommandHandler(
            IPedidoDomainService pedidoDomainService,
            IPedidoServiceHandler pedidoServiceHandler,
            IMapper mapper,
            ILogger<CalcularPedidoCommandHandler> logger)
        {
            this.pedidoDomainService = pedidoDomainService;
            this.pedidoServiceHandler = pedidoServiceHandler;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<bool> Handle(CalcularPedidoCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Calculando o imposto do pedido. CorrelationId {request?.CorrelationId}");

            var enviouImposto = false;

            try
            {
                var valorImposto = await pedidoDomainService.CalcularImposto(request?.Itens?.Sum(s => s?.Quantidade * s?.Valor) ?? 0);

                logger.LogDebug($"Valor do imposto {valorImposto:0.00}. CorrelationId {request?.CorrelationId}");

                var pedido = mapper.Map<Pedido>(request);

                pedido.Imposto = valorImposto;

                enviouImposto = await pedidoServiceHandler.EnviarImposto(pedido, request?.CorrelationId, cancellationToken);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error ao calcular o pedido. CorrelationId {request?.CorrelationId}.");

                return await Task.FromResult<bool>(false);
            }

            return await Task.FromResult<bool>(enviouImposto);
        }
    }
}