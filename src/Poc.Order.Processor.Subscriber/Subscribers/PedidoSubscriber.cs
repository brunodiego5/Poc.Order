using AutoMapper;
using Microsoft.Extensions.Logging;
using Poc.Order.Processor.Domain.Interfaces;
using Poc.Order.Processor.Infrastructure.Publisher.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Poc.Order.Processor.Subscriber.Subscribers
{
    public class PedidoSubscriber : IPedidoSubscriber
    {
        private const string Exchange = "poc.order.queue";
        private const string RoutingKey = "pedido.criado";
        private const string Queue = "poc.order.queue.message";
        private readonly IConnection connection;
        private readonly IModel model;
        private readonly ILogger<PedidoSubscriber> logger;

        public PedidoSubscriber(IConnection connection, ILogger<PedidoSubscriber> logger)
        {
            this.connection = connection;
            this.logger = logger;
            this.model = connection.CreateModel();

            model.ExchangeDeclare(Exchange, ExchangeType.Direct, durable: true);
            model.QueueDeclare(Queue, durable: true, exclusive: false, autoDelete: false);
            model.QueueBind(Queue, Exchange, RoutingKey);
        }

        public Task ConsumePedido(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(model);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    var message = JsonSerializer.Deserialize<PedidoMessage>(json);

                    logger.LogInformation($"Pedido recebido {message?.PedidoId}. CorrelationId {message?.CorrelationId}.");
                }
                catch (Exception ex)
                {
                    logger.LogInformation(ex, $"Erro ao receber pedido.");
                }
            };

            model.BasicConsume(Queue, autoAck: true, consumer);

            return Task.CompletedTask;
        }
    }
}
