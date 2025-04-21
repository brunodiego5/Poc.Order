using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Poc.Order.Api.Domain.Enums;
using Poc.Order.Api.Domain.Interfaces;

namespace Poc.Order.Api.Application.Commands.UpdateImpostoPedido
{
    public class UpdateImpostoPedidoCommandHandler : IRequestHandler<UpdateImpostoPedidoCommand, UpdateImpostoPedidoCommandResponse>
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

        public async Task<UpdateImpostoPedidoCommandResponse> Handle(UpdateImpostoPedidoCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Recebemos o pedido {request?.PedidoId} e imposto {request?.Imposto}. CorrelationId: {request?.CorrelationId}");

            var response = new UpdateImpostoPedidoCommandResponse() { Id = request.PedidoId };

            try
            {

                var result = validator.Validate(request);

                if (!result.IsValid)
                    throw new ValidationException(result.Errors);

                await pedidoRepository.UpdateImpostoPedidoAsync(request.PedidoId, request.Imposto, cancellationToken);
                logger.LogInformation($"Imposto do pedido atualizado. CorrelationId: {request?.CorrelationId}");

                response.Status = StatusPedido.Concluido;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao atualizar imposto do pedido. CorrelationId: {request?.CorrelationId}");
                response.Status = StatusPedido.Cancelado;
            }

            return await Task.FromResult(response);
        }
    }
}
