using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Poc.Order.Api.Domain.Enums;

namespace Poc.Order.Api.Infrastructure.Data.Models
{
    public class PedidoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int PedidoId { get; set; }

        public int ClientId { get; set; }

        public decimal Imposto { get; set; }

        public required IList<ItemPedidoModel> Itens { get; set; }

        public StatusPedido Status { get; set; }
    }

    public record ItemPedidoModel(int ProdutoId, decimal Quantidade, decimal Valor);
}
