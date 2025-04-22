using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Poc.Order.Processor.Application.Commands.CalcularPedido;
using Poc.Order.Processor.Domain.Interfaces;
using Poc.Order.Processor.Infrastructure.Publisher.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

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
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public PedidoSubscriber(IConnection connection, ILogger<PedidoSubscriber> logger, IMediator mediator, IMapper mapper)
        {
            this.connection = connection;
            this.logger = logger;
            this.mediator = mediator;
            this.mapper = mapper;
            this.model = connection.CreateModel();

            model.ExchangeDeclare(Exchange, ExchangeType.Direct, durable: true);
            model.QueueDeclare(Queue, durable: true, exclusive: false, autoDelete: false);
            model.QueueBind(Queue, Exchange, RoutingKey);
        }

        public Task ConsumePedido(CancellationToken cancellationToken)
        {
            PedidoMessage message = null;
            var enviouImposto = false;

            var consumer = new EventingBasicConsumer(model);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    logger.LogDebug($"Json Pedido recebido {json ?? string.Empty}. CorrelationId {message?.CorrelationId}.");

                    message = JsonSerializer.Deserialize<PedidoMessage>(json);

                    logger.LogInformation($"Pedido recebido {message?.PedidoId}. CorrelationId {message?.CorrelationId}.");

                    var command = mapper.Map<CalcularPedidoCommand>(message);

                    enviouImposto = await mediator.Send(command, cancellationToken);

                    logger.LogInformation($"Pedido processado com sucesso: {enviouImposto}. CorrelationId {message?.CorrelationId}.");
                }
                catch (Exception ex)
                {
                    logger.LogInformation(ex, $"Erro ao processar pedido. CorrelationId {message?.CorrelationId}.");
                    enviouImposto = false;
                }
            };

            model.BasicConsume(Queue, autoAck: true, consumer);

            return Task.CompletedTask;
        }
    }
}
