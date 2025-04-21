using Poc.Order.Processor.Domain.Enums;

namespace Poc.Order.Processor.Service.Contracts
{
    public class EnviarImpostoResponse
    {
        public int Id { get; set; }

        public StatusPedido Status { get; set; }
    }
}
