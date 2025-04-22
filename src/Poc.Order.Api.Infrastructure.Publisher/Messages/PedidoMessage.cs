namespace Poc.Order.Api.Infrastructure.Publisher.Messages
{
    public class PedidoMessage
    {
        public string CorrelationId { get; set; }

        public int PedidoId { get; set; }

        public required IList<ItemPedidoMessage> Itens { get; set; }
    }

    public record ItemPedidoMessage(decimal Quantidade, decimal Valor);
}
