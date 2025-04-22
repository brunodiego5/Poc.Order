using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Poc.Order.Api.Application.Commands.UpdateImpostoPedido;
using Poc.Order.Api.Domain.Enums;
using Poc.Order.Api.Domain.Interfaces;

namespace Poc.Order.Api.Test.Poc.Order.Api.Application.Test.Commands.UpdateImpostoPedido
{
    public class UpdateImpostoPedidoCommandHandlerTests
    {
        private readonly Mock<IPedidoRepository> pedidoRepository;
        private readonly Mock<IValidator<UpdateImpostoPedidoCommand>> validator;
        private readonly Mock<ILogger<UpdateImpostoPedidoCommandHandler>> logger;

        public UpdateImpostoPedidoCommandHandlerTests()
        {
            pedidoRepository = new Mock<IPedidoRepository>();
            validator = new Mock<IValidator<UpdateImpostoPedidoCommand>>();
            logger = new Mock<ILogger<UpdateImpostoPedidoCommandHandler>>();
        }

        [Fact]
        public async Task SalvarPedido_ComCamposObrigatorios_DeveSalvarComSucesso()
        {
            var command = new UpdateImpostoPedidoCommand()
            {
                PedidoId = 1,
                Imposto = 10
            };

            var expectedResponse = new UpdateImpostoPedidoCommandResponse()
            {
                Id = 1,
                Status = StatusPedido.Concluido
            };

            pedidoRepository.Setup(r => r.UpdateImpostoPedidoAsync(command.PedidoId, command.Imposto, default(CancellationToken)))
                .Returns(Task.CompletedTask);

            validator.Setup(v => v.Validate(command))
                .Returns(new ValidationResult());

            var handler = new UpdateImpostoPedidoCommandHandler(pedidoRepository.Object, validator.Object, logger.Object);

            var response = await handler.Handle(command, default(CancellationToken));

            Assert.Equal(expectedResponse.Id, response.Id);
            Assert.Equal(expectedResponse.Status, response.Status);
            pedidoRepository.Verify(v => v.UpdateImpostoPedidoAsync(command.PedidoId, command.Imposto, default(CancellationToken)),
                Times.Once());
        }

        [Fact]
        public async Task SalvarPedido_SemCamposObrigatorios_DeveFalhar()
        {
            var command = new UpdateImpostoPedidoCommand()
            {
                PedidoId = 1,
                Imposto = -1
            };

            var expectedResponse = new UpdateImpostoPedidoCommandResponse()
            {
                Id = 1,
                Status = StatusPedido.Cancelado
            };

            validator.Setup(v => v.Validate(command))
                .Returns(new ValidationResult(new List<ValidationFailure>
                { 
                    new ValidationFailure("Imposto", "Valor do Imposto não pode ser negativo.")
                }));

            var handler = new UpdateImpostoPedidoCommandHandler(pedidoRepository.Object, validator.Object, logger.Object);

            var response = await handler.Handle(command, default(CancellationToken));

            Assert.Equal(expectedResponse.Id, response.Id);
            Assert.Equal(expectedResponse.Status, response.Status);
            pedidoRepository.Verify(v => v.UpdateImpostoPedidoAsync(command.PedidoId, command.Imposto, default(CancellationToken)),
                Times.Never());
        }
    }
}
