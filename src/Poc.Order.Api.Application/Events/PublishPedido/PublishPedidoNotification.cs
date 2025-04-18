using MediatR;
using Poc.Order.Api.Domain.Entities;

namespace Poc.Order.Api.Application.Events.PublishPedido
{
    public class PublishPedidoNotification : INotification
    {
        public string CorrelationId { get; set; }

        public Pedido Pedido { get; set; }
    }
}
