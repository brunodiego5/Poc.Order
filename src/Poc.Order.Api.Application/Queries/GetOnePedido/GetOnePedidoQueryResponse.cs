using Poc.Order.Api.Domain.Enums;

namespace Poc.Order.Api.Application.Queries.GetOnePedido
{
    public class GetOnePedidoQueryResponse
    {
        public string Id { get; set; }

        public int PedidoId { get; set; }

        public int ClientId { get; set; }

        public double Imposto { get; set; }

        public required IList<ItemPedidoQuery> Itens { get; set; }

        public StatusPedido Status { get; set; }
    }

    public record ItemPedidoQuery(int ProdutoId, double Quantidade, double Valor);
}
