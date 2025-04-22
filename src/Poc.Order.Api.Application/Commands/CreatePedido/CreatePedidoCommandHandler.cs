using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Poc.Order.Api.Application.Events.PublishPedido;
using Poc.Order.Api.Domain.Entities;
using Poc.Order.Api.Domain.Enums;
using Poc.Order.Api.Domain.Interfaces;

namespace Poc.Order.Api.Application.Commands.CreatePedido
{
    public class CreatePedidoCommandHandler : IRequestHandler<CreatePedidoCommand, CreatePedidoCommandResponse>
    {
        private readonly IPedidoRepository pedidoRepository;
        private readonly IValidator<CreatePedidoCommand> validator;
        private readonly IMapper mapper;
        private readonly ILogger<CreatePedidoCommandHandler> logger;
        private readonly IMediator mediator;

        public CreatePedidoCommandHandler(
            IPedidoRepository pedidoRepository, 
            IValidator<CreatePedidoCommand> validator,
            IMapper mapper,
            ILogger<CreatePedidoCommandHandler> logger,
            IMediator mediator)
        {
            this.pedidoRepository = pedidoRepository;
            this.validator = validator;
            this.mapper = mapper;
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task<CreatePedidoCommandResponse> Handle(CreatePedidoCommand request, CancellationToken cancellationToken)
        {
            string correlationId = Guid.NewGuid().ToString();

            logger.LogInformation($"Recebemos o pedido {request?.PedidoId} e cliente {request?.ClientId}. CorrelationId: {correlationId}");

            var response = new CreatePedidoCommandResponse()
            {
                PedidoId = request.PedidoId
            };

            try
            {

                var result = validator.Validate(request);

                if (!result.IsValid)
                    throw new ValidationException(result.Errors);

                var pedido = mapper.Map<Pedido>(request);

                await pedidoRepository.AddPedidoAsync(pedido, cancellationToken);
                logger.LogInformation($"Pedido salvo. CorrelationId: {correlationId}");

                response.Status = StatusPedido.Criado;

                _ = mediator.Publish(
                    new PublishPedidoNotification
                    {
                        CorrelationId = correlationId,
                        Pedido = pedido
                    }, cancellationToken);
            }
            catch (Exception ex)
            {
                response.Status = StatusPedido.Erro;
                logger.LogError(ex, $"Erro ao criar pedido. CorrelationId: {correlationId}");
            }

            return response;
        }
    }
}
