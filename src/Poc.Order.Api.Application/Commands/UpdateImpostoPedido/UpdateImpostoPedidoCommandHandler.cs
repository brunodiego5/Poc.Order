using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Poc.Order.Api.Domain.Entities;
using Poc.Order.Api.Domain.Enums;
using Poc.Order.Api.Domain.Interfaces;

namespace Poc.Order.Api.Application.Commands.UpdateImpostoPedido
{
    public class UpdateImpostoPedidoCommandHandler : IRequestHandler<UpdateImpostoPedidoCommand, Unit>
    {
        private readonly IPedidoRepository pedidoRepository;
        private readonly IValidator<UpdateImpostoPedidoCommand> validator;
        private readonly ILogger<UpdateImpostoPedidoCommandHandler> logger;

        public UpdateImpostoPedidoCommandHandler(
            IPedidoRepository pedidoRepository, 
            IValidator<UpdateImpostoPedidoCommand> validator,
            ILogger<UpdateImpostoPedidoCommandHandler> logger)
        {
            this.pedidoRepository = pedidoRepository;
            this.validator = validator;
            this.logger = logger;
        }

        public async Task<Unit> Handle(UpdateImpostoPedidoCommand request, CancellationToken cancellationToken)
        {
            string correlationId = Guid.NewGuid().ToString();

            logger.LogInformation($"Recebemos o pedido {request?.PedidoId} e imposto {request?.Imposto}. CorrelationId: {correlationId}");

            try
            {

                var result = validator.Validate(request);

                if (!result.IsValid)
                    throw new ValidationException(result.Errors);

                await pedidoRepository.UpdateImpostoPedidoAsync(request.PedidoId, request.Imposto, cancellationToken);
                logger.LogInformation($"Imposto do pedido atualizado. CorrelationId: {correlationId}");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao atualizar imposto do pedido. CorrelationId: {correlationId}");
            }

            return Unit.Value;
        }
    }
}
