using MediatR;

namespace Poc.Order.Api.Application.Queries.GetOnePedido
{
    public class GetOnePedidoQuery : IRequest<GetOnePedidoQueryResponse>
    {
        public int PedidoId { get; set; }
    }
}
