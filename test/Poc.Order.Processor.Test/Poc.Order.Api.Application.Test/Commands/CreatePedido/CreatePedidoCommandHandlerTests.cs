using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Poc.Order.Api.Application.Commands.CreatePedido;
using Poc.Order.Api.Domain.Entities;
using Poc.Order.Api.Domain.Enums;
using Poc.Order.Api.Domain.Interfaces;

namespace Poc.Order.Api.Test.Poc.Order.Api.Application.Test.Commands.CreatePedido
{
    public class CreatePedidoCommandHandlerTests
    {
        private readonly Mock<IPedidoRepository> pedidoRepository;
        private readonly Mock<IValidator<CreatePedidoCommand>> validator;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<ILogger<CreatePedidoCommandHandler>> logger;

        public CreatePedidoCommandHandlerTests()
        {
            pedidoRepository = new Mock<IPedidoRepository>();
            validator = new Mock<IValidator<CreatePedidoCommand>>();
            mapper = new Mock<IMapper>();
            logger = new Mock<ILogger<CreatePedidoCommandHandler>>();
        }

        [Fact]
        public async Task SalvarPedido_ComCamposObrigatorios_DeveSalvarComSucesso()
        {
            var command = new CreatePedidoCommand()
            {
                PedidoId = 1,
                ClientId = 1,
                Itens = new List<ItemPedidoCommand>()
            };

            var pedido = new Pedido()
            {
                PedidoId = 1,
                ClientId = 1,
                Itens = new List<ItemPedido>()
            };

            pedidoRepository.Setup(r => r.AddPedidoAsync(It.IsAny<Pedido>(), default(CancellationToken)))
                .Returns(Task.CompletedTask);

            validator.Setup(v => v.Validate(command))
                .Returns(new ValidationResult());

            mapper.Setup(m => m.Map<Pedido>(command))
                .Returns(pedido);

            var handler = new CreatePedidoCommandHandler(pedidoRepository.Object, validator.Object, mapper.Object, logger.Object);



            var response = await handler.Handle(command, default(CancellationToken));


            Assert.Equal(command.PedidoId, response.Id);
            Assert.Equal(StatusPedido.Criado, response.Status);
        }

        [Fact]
        public async Task SalvarPedido_SemCamposObrigatorios_DeveFalhar()
        {
            var command = new CreatePedidoCommand()
            {
                PedidoId = 1,
                Itens = new List<ItemPedidoCommand>()
            };

            validator.Setup(v => v.Validate(command))
                .Returns(new ValidationResult(new List<ValidationFailure>
                { 
                    new ValidationFailure("ClientId", "Cliente é obrigatório")
                }));

            var handler = new CreatePedidoCommandHandler(pedidoRepository.Object, validator.Object, mapper.Object, logger.Object);

            var response = await handler.Handle(command, default(CancellationToken));


            Assert.Equal(StatusPedido.Erro, response.Status);
        }
    }
}
