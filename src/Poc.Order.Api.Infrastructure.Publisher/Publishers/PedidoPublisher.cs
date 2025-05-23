﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Poc.Order.Api.Domain.Entities;
using Poc.Order.Api.Domain.Interfaces;
using Poc.Order.Api.Infrastructure.Publisher.Messages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Poc.Order.Api.Infrastructure.Publisher.Publishers
{
    public class PedidoPublisher : IPedidoPublisher, IDisposable
    {
        private const string Exchange = "poc.order.queue";
        private const string RoutingKey = "pedido.criado";
        private readonly IConnection connection;
        private readonly IModel model;
        private readonly IMapper mapper;
        private readonly ILogger<PedidoPublisher> logger;

        public PedidoPublisher(IConnection connection, IMapper mapper, ILogger<PedidoPublisher> logger)
        {
            this.mapper = mapper;
            this.connection = connection;
            this.logger = logger;
            this.model = connection.CreateModel();

            model.ExchangeDeclare(Exchange, ExchangeType.Direct, durable: true);
        }

        public Task PublishPedido(Pedido pedido, string correlationId, CancellationToken cancellationToken)
        {
            var message = mapper.Map<PedidoMessage>(pedido);
            message.CorrelationId = correlationId;

            var body = JsonSerializer.SerializeToUtf8Bytes(message);

            logger.LogDebug($"Json Pedido enviado {Encoding.UTF8.GetString(body) ?? string.Empty}. CorrelationId {message?.CorrelationId}.");

            model.BasicPublish(
                exchange: Exchange,
                routingKey: RoutingKey,
                mandatory: false,
                basicProperties: null,
                body: body);

            return Task.CompletedTask;
        }

        public void Dispose() => model?.Dispose();
    }
}
