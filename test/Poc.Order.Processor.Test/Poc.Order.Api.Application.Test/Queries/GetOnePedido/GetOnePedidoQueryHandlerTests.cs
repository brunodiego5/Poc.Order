using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Poc.Order.Api.Application.Queries.GetOnePedido;
using Poc.Order.Api.Domain.Entities;
using Poc.Order.Api.Domain.Enums;
using Poc.Order.Api.Domain.Interfaces;

namespace Poc.Order.Api.Test.Poc.Order.Api.Application.Test.Queries.GetOnePedido
{
    public class GetOnePedidoQueryHandlerTests
    {
        private readonly Mock<IPedidoRepository> pedidoRepository;
        private readonly Mock<IValidator<GetOnePedidoQuery>> validator;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<ILogger<GetOnePedidoQueryHandler>> logger;

        public GetOnePedidoQueryHandlerTests()
        {
            pedidoRepository = new Mock<IPedidoRepository>();
            validator = new Mock<IValidator<GetOnePedidoQuery>>();
            mapper = new Mock<IMapper>();
            logger = new Mock<ILogger<GetOnePedidoQueryHandler>>();
        }

        [Fact]
        public async Task ObterPedido_ComCamposObrigatorios_DeveObterComSucesso()
        {
            var query = new GetOnePedidoQuery()
            {
                PedidoId = 1
            };

            var pedido = new Pedido()
            {
                PedidoId = 1,
                ClientId = 1,
                Itens = new List<ItemPedido>(),
                Status = StatusPedido.Criado
            };

            var responseExpect = new GetOnePedidoQueryResponse()
            {
                PedidoId = 1,
                ClientId = 1,
                Itens = new List<ItemPedidoQuery>(),
                Status = StatusPedido.Criado
            };

            pedidoRepository.Setup(r => r.GetPedidoByIdAsync(It.IsAny<int>(), default(CancellationToken)))
                .ReturnsAsync(pedido);

            validator.Setup(v => v.Validate(query))
                .Returns(new ValidationResult());

            mapper.Setup(m => m.Map<GetOnePedidoQueryResponse>(pedido))
                .Returns(responseExpect);

            var handler = new GetOnePedidoQueryHandler(pedidoRepository.Object, validator.Object, mapper.Object, logger.Object);

            var response = await handler.Handle(query, default(CancellationToken));

            Assert.Same(responseExpect, response);
            pedidoRepository.Verify(r => r.GetPedidoByIdAsync(query.PedidoId, default(CancellationToken)), 
                Times.Once());
            validator.Verify(r => r.Validate(query), Times.Once());
            Assert.Equal(StatusPedido.Criado, response.Status);
        }

        [Fact]
        public async Task SalvarPedido_SemCamposObrigatorios_DeveFalhar()
        {
            var query = new GetOnePedidoQuery()
            {
                PedidoId = 0
            };

            validator.Setup(v => v.Validate(query))
                .Returns(new ValidationResult(new List<ValidationFailure>
                { 
                    new ValidationFailure("PedidoId", "Número do Pedido é obrigatório")
                }));

            var handler = new GetOnePedidoQueryHandler(pedidoRepository.Object, validator.Object, mapper.Object, logger.Object);

            var response = await handler.Handle(query, default(CancellationToken));

            Assert.Null(response);
            pedidoRepository.Verify(r => r.GetPedidoByIdAsync(query.PedidoId, default(CancellationToken)),
                Times.Never());
        }
    }
}
