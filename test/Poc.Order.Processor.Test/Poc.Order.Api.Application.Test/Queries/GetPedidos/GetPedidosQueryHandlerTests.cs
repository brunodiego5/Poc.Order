using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Poc.Order.Api.Application.Queries.GetPedidos;
using Poc.Order.Api.Domain.Entities;
using Poc.Order.Api.Domain.Enums;
using Poc.Order.Api.Domain.Interfaces;

namespace Poc.Order.Api.Test.Poc.Order.Api.Application.Test.Queries.GetPedidos
{
    public class GetPedidosQueryHandlerTests
    {
        private readonly Mock<IPedidoRepository> pedidoRepository;
        private readonly Mock<IValidator<GetPedidosQuery>> validator;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<ILogger<GetPedidosQueryHandler>> logger;

        public GetPedidosQueryHandlerTests()
        {
            pedidoRepository = new Mock<IPedidoRepository>();
            validator = new Mock<IValidator<GetPedidosQuery>>();
            mapper = new Mock<IMapper>();
            logger = new Mock<ILogger<GetPedidosQueryHandler>>();
        }

        [Fact]
        public async Task ObterPedido_ComCamposObrigatorios_DeveObterComSucesso()
        {
            var query = new GetPedidosQuery()
            {
                Status = StatusPedido.Criado
            };

            var pedidos = new List<Pedido>()
            {
                new Pedido()
                {
                    PedidoId = 1,
                    ClientId = 1,
                    Itens = new List<ItemPedido>(),
                    Status = StatusPedido.Criado
                }
            };

            var responseExpect = new List<GetPedidosQueryResponse>()
            {
                new GetPedidosQueryResponse()
                {
                    PedidoId = 1,
                    ClientId = 1,
                    Itens = new List<ItemPedidosQuery>(),
                    Status = StatusPedido.Criado
                }
            };

            pedidoRepository.Setup(r => r.GetPedidosByStatusAsync(StatusPedido.Criado, default(CancellationToken)))
                .ReturnsAsync(pedidos);

            validator.Setup(v => v.Validate(query))
                .Returns(new ValidationResult());

            mapper.Setup(m => m.Map<List<GetPedidosQueryResponse>>(pedidos))
                .Returns(responseExpect);

            var handler = new GetPedidosQueryHandler(pedidoRepository.Object, validator.Object, mapper.Object, logger.Object);

            var response = await handler.Handle(query, default(CancellationToken));

            Assert.Same(responseExpect, response);
            pedidoRepository.Verify(r => r.GetPedidosByStatusAsync(query.Status, default(CancellationToken)), 
                Times.Once());
            validator.Verify(r => r.Validate(query), Times.Once());
            Assert.Single(response);
        }

        [Fact]
        public async Task SalvarPedido_SemCamposObrigatorios_DeveFalhar()
        {
            var query = new GetPedidosQuery();

            validator.Setup(v => v.Validate(query))
                .Returns(new ValidationResult(new List<ValidationFailure>
                { 
                    new ValidationFailure("StatusPedido", "Status do Pedido é obrigatório")
                }));

            var handler = new GetPedidosQueryHandler(pedidoRepository.Object, validator.Object, mapper.Object, logger.Object);

            var response = await handler.Handle(query, default(CancellationToken));

            Assert.Null(response);
            pedidoRepository.Verify(r => r.GetPedidosByStatusAsync(query.Status, default(CancellationToken)),
                Times.Never());
        }
    }
}
