namespace Poc.Order.Processor.Service.Contracts
{
    public class EnviarImpostoRequest
    {
        public string CorrelationId { get; set; }

        public int PedidoId { get; set; }

        public decimal Imposto { get; set; }
    }
}
