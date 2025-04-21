using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poc.Order.Processor.Domain.Interfaces;

namespace Poc.Order.Processor.Workers
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IPedidoSubscriber subscriber;

        public Worker(ILogger<Worker> logger, IPedidoSubscriber subscriber)
        {
            this.logger = logger;
            this.subscriber = subscriber;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Worker iniciado.");
            await subscriber.ConsumePedido(stoppingToken);
        }
    }
}
