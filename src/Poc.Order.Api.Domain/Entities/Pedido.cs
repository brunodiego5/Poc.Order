using Poc.Order.Api.Domain.Enums;

namespace Poc.Order.Api.Domain.Entities
{

	public class Pedido
	{

        public int PedidoId { get; set; }

		public int ClientId { get; set; }

		public double Imposto { get; set; }

        public required IList<ItemPedido> Itens { get; set; }

		public StatusPedido Status { get; set; } = StatusPedido.Criado;
        
		public Pedido()
        {
        }
	}

	public record ItemPedido(int ProdutoId, double Quantidade, double Valor);
}