using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Poc.Order.Processor.Application.Commands.CalcularPedido;
using Poc.Order.Processor.Domain.Entities;
using Poc.Order.Processor.Domain.Interfaces;

namespace Poc.Order.Processor.Test.Poc.Order.Processor.Application.Test.Commands.CalcularPedido
{
    public class CalcularPedidoCommandHandlerTests
    {
        private readonly Mock<IPedidoDomainService> pedidoDomainService;

        private readonly Mock<IPedidoServiceHandler> pedidoServiceHandler;

        private readonly Mock<IMapper> mapper;

        private readonly Mock<ILogger<CalcularPedidoCommandHandler>> logger;

        public CalcularPedidoCommandHandlerTests()
        {
            pedidoDomainService = new Mock<IPedidoDomainService>();
            pedidoServiceHandler = new Mock<IPedidoServiceHandler>();
            mapper = new Mock<IMapper>();
            logger = new Mock<ILogger<CalcularPedidoCommandHandler>>();
        }

        [Fact]
        public async Task CalcularPedido_ParametrosValidos_DeveRetornarSucesso()
        {
            var correlationId = Guid.NewGuid().ToString();

            var command = new CalcularPedidoCommand()
            {
                CorrelationId = correlationId,
                PedidoId = 1,
                Itens = new List<ItemPedidoCommand>()
                {
                    new ItemPedidoCommand(Quantidade: 3, Valor: 4)
                }
            };

            var valorImposto = command.Itens.Sum(s => s.Quantidade * s.Valor);

            var pedido = new Pedido()
            {
                PedidoId = 1,
                Imposto = valorImposto,
                Itens = new List<ItemPedido>()
                {
                    new ItemPedido(Quantidade: 3, Valor: 4)
                }

            };

            var expectedReturn = true;

            pedidoDomainService.Setup(d => d.CalcularImposto(valorImposto))
                .ReturnsAsync(valorImposto * 0.30m);

            mapper.Setup(m => m.Map<Pedido>(command))
                .Returns(pedido);

            pedidoServiceHandler.Setup(s => s.EnviarImposto(pedido, correlationId, default(CancellationToken)))
                .ReturnsAsync(expectedReturn);

            var handler = new CalcularPedidoCommandHandler(pedidoDomainService.Object, pedidoServiceHandler.Object, mapper.Object, logger.Object);

            var response = await handler.Handle(command, default(CancellationToken));

            Assert.Equal(expectedReturn, response);
            pedidoDomainService.Verify(v => v.CalcularImposto(valorImposto),
                Times.Once());

            pedidoServiceHandler.Verify(v => v.EnviarImposto(pedido, correlationId, default(CancellationToken)),
                Times.Once());
        }

        [Fact]
        public async Task CalcularPedido_ErroNoEnvio_DeveRetornarFalse()
        {
            var correlationId = Guid.NewGuid().ToString();

            var command = new CalcularPedidoCommand()
            {
                CorrelationId = correlationId,
                PedidoId = 1,
                Itens = new List<ItemPedidoCommand>()
                {
                    new ItemPedidoCommand(Quantidade: 3, Valor: 4)
                }
            };

            var valorImposto = command.Itens.Sum(s => s.Quantidade * s.Valor);

            var pedido = new Pedido()
            {
                PedidoId = 1,
                Imposto = valorImposto,
                Itens = new List<ItemPedido>()
                {
                    new ItemPedido(Quantidade: 3, Valor: 4)
                }

            };

            var expectedReturn = false;

            pedidoDomainService.Setup(d => d.CalcularImposto(valorImposto))
                .ReturnsAsync(valorImposto * 0.30m);

            mapper.Setup(m => m.Map<Pedido>(command))
                .Returns(pedido);

            pedidoServiceHandler.Setup(s => s.EnviarImposto(pedido, correlationId, default(CancellationToken)))
                .ReturnsAsync(expectedReturn);

            var handler = new CalcularPedidoCommandHandler(pedidoDomainService.Object, pedidoServiceHandler.Object, mapper.Object, logger.Object);

            var response = await handler.Handle(command, default(CancellationToken));

            Assert.Equal(expectedReturn, response);
            pedidoDomainService.Verify(v => v.CalcularImposto(valorImposto),
                Times.Once());

            pedidoServiceHandler.Verify(v => v.EnviarImposto(pedido, correlationId, default(CancellationToken)),
                Times.Once());
        }
    }
}