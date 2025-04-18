using Poc.Order.Api.Domain.Enums;

namespace Poc.Order.Api.Infrastructure.Publisher.Messages
{
    public class PedidoMessage
    {
        public string CorrelationId { get; set; }

        public int PedidoId { get; set; }

        public int ClientId { get; set; }

        public double Imposto { get; set; }

        public required IList<ItemPedidoMessage> Itens { get; set; }

        public StatusPedido Status { get; set; } = StatusPedido.Criado;
    }

    public record ItemPedidoMessage(int ProdutoId, double Quantidade, double Valor);
}
