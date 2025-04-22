using Poc.Order.Api.Domain.Entities;
using Poc.Order.Api.Domain.Enums;

namespace Poc.Order.Api.Test.Poc.Order.Api.Domain.Test.Entities
{
    public class PedidoTests
    {

        [Fact]
        public void Pedido_DeveSerStatusCriado_QuandoCriado()
        {
            var pedido = new Pedido()
            {
                Itens = new List<ItemPedido>()
            };

            pedido.PedidoId = 1;
            pedido.ClientId = 1;

            Assert.Equal(StatusPedido.Criado, pedido.Status);
        }
    }
}
