using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Poc.Order.Api.Domain.Interfaces;

namespace Poc.Order.Api.Application.Queries.GetPedidos
{
    public class GetPedidosQueryHandler : IRequestHandler<GetPedidosQuery, IList<GetPedidosQueryResponse>>
    {
        private readonly IPedidoRepository pedidoRepository;
        private readonly IValidator<GetPedidosQuery> validator;
        private readonly IMapper mapper;
        private readonly ILogger<GetPedidosQueryHandler> logger;

        public GetPedidosQueryHandler(
            IPedidoRepository pedidoRepository, 
            IValidator<GetPedidosQuery> validator,
            IMapper mapper,
            ILogger<GetPedidosQueryHandler> logger)
        {
            this.pedidoRepository = pedidoRepository;
            this.validator = validator;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IList<GetPedidosQueryResponse>> Handle(GetPedidosQuery request, CancellationToken cancellationToken)
        {
            string correlationId = Guid.NewGuid().ToString();

            logger.LogInformation($"Consultar pedidos pelo status {request?.Status}. CorrelationId: {correlationId}");

            List<GetPedidosQueryResponse> response = null;

            try
            {
                var result = validator.Validate(request);

                if (!result.IsValid)
                    throw new ValidationException(result.Errors);

                var pedidos = await pedidoRepository.GetPedidosByStatusAsync(request.Status, cancellationToken);
                response = mapper.Map<List<GetPedidosQueryResponse>>(pedidos);

                logger.LogInformation($"Total de Pedidos encontrado: {response?.Count ?? 0}. CorrelationId: {correlationId}");            

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao consultar pedidos. CorrelationId: {correlationId}");
            }

            return response;
        }
    }
}
