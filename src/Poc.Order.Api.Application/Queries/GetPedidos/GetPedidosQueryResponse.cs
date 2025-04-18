using Poc.Order.Api.Domain.Enums;

namespace Poc.Order.Api.Application.Queries.GetPedidos
{
    public class GetPedidosQueryResponse
    {
        public string Id { get; set; }

        public int PedidoId { get; set; }

        public int ClientId { get; set; }

        public double Imposto { get; set; }

        public required IList<ItemPedidosQuery> Itens { get; set; }

        public StatusPedido Status { get; set; }
    }

    public record ItemPedidosQuery(int ProdutoId, double Quantidade, double Valor);
}
