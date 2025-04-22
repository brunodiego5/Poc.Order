using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Poc.Order.Api.Domain.Interfaces;

namespace Poc.Order.Api.Application.Queries.GetOnePedido
{
    public class GetOnePedidoQueryHandler : IRequestHandler<GetOnePedidoQuery, GetOnePedidoQueryResponse>
    {
        private readonly IPedidoRepository pedidoRepository;
        private readonly IValidator<GetOnePedidoQuery> validator;
        private readonly IMapper mapper;
        private readonly ILogger<GetOnePedidoQueryHandler> logger;

        public GetOnePedidoQueryHandler(
            IPedidoRepository pedidoRepository, 
            IValidator<GetOnePedidoQuery> validator,
            IMapper mapper,
            ILogger<GetOnePedidoQueryHandler> logger)
        {
            this.pedidoRepository = pedidoRepository;
            this.validator = validator;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<GetOnePedidoQueryResponse> Handle(GetOnePedidoQuery request, CancellationToken cancellationToken)
        {
            string correlationId = Guid.NewGuid().ToString();

            logger.LogInformation($"Consultar o pedido {request?.PedidoId}. CorrelationId: {correlationId}");

            GetOnePedidoQueryResponse response = null;

            try
            {
                var result = validator.Validate(request);

                if (!result.IsValid)
                    throw new ValidationException(result.Errors);

                var pedido = await pedidoRepository.GetPedidoByIdAsync(request.PedidoId, cancellationToken);
                response = mapper.Map<GetOnePedidoQueryResponse>(pedido);

                logger.LogInformation($"Pedido encontrado: {response?.PedidoId}. CorrelationId: {correlationId}");            

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao consultar pedido. CorrelationId: {correlationId}");
            }

            return response;
        }
    }
}
