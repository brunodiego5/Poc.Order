using MediatR;
using Microsoft.Extensions.Logging;
using Poc.Order.Api.Domain.Interfaces;

namespace Poc.Order.Api.Application.Events.PublishPedido
{
    public class PublishPedidoNotificationHandler : INotificationHandler<PublishPedidoNotification>
    {
        private readonly IPedidoPublisher publisher;
        private readonly ILogger<PublishPedidoNotificationHandler> logger;

        public PublishPedidoNotificationHandler(IPedidoPublisher publisher, ILogger<PublishPedidoNotificationHandler> logger)
        {
            this.publisher = publisher;
            this.logger = logger;
        }

        public async Task Handle(PublishPedidoNotification notification, CancellationToken cancellationToken)
        {

            try
            {
                await publisher.PublishPedido(notification.Pedido, notification.CorrelationId, cancellationToken);
                logger.LogInformation($"Pedido {notification?.Pedido?.PedidoId} publicado. CorrelationId {notification?.CorrelationId}.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao publicar o Pedido {notification?.Pedido?.PedidoId}. CorrelationId {notification?.CorrelationId}.");
            }         
        }
    }
}
