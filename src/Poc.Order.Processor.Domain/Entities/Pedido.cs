namespace Poc.Order.Processor.Domain.Entities
{

	public class Pedido
	{
        public int PedidoId { get; set; }

        public decimal Imposto { get; set; }

        public required IList<ItemPedido> Itens { get; set; }
	}

	public record ItemPedido(decimal Quantidade, decimal Valor);
}