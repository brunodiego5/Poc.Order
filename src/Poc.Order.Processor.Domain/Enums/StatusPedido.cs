using System.Text.Json.Serialization;

namespace Poc.Order.Processor.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusPedido
    {
        Criado = 1,
        Processando = 2,
        Concluido = 3,
        Cancelado = 4,
        Erro = 5
    }
}