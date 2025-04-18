using MediatR;
using Poc.Order.Api.Domain.Enums;

namespace Poc.Order.Api.Application.Queries.GetPedidos
{
    public class GetPedidosQuery : IRequest<IList<GetPedidosQueryResponse>>
    {
        public StatusPedido Status { get; set; }
    }
}
