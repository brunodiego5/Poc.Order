using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
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

        public CreatePedidoCommandHandler(
            IPedidoRepository pedidoRepository, 
            IValidator<CreatePedidoCommand> validator,
            IMapper mapper,
            ILogger<CreatePedidoCommandHandler> logger)
        {
            this.pedidoRepository = pedidoRepository;
            this.validator = validator;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<CreatePedidoCommandResponse> Handle(CreatePedidoCommand request, CancellationToken cancellationToken)
        {
            string correlationId = Guid.NewGuid().ToString();

            logger.LogInformation($"Recebemos o pedido {request?.PedidoId} e cliente {request?.ClientId}. CorrelationId: {correlationId}");

            var response = new CreatePedidoCommandResponse()
            {
                Id = request.PedidoId
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
