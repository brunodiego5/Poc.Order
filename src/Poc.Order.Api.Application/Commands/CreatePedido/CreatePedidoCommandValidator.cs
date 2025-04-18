using FluentValidation;

namespace Poc.Order.Api.Application.Commands.CreatePedido
{
    public class CreatePedidoCommandValidator : AbstractValidator<CreatePedidoCommand>
    {
        public CreatePedidoCommandValidator()
        {
            RuleFor(x => x.PedidoId)
                .NotEmpty().WithMessage("Número do Pedido é obrigatório.")
                .GreaterThan(0).WithMessage("Número do Pedido deve ser maior que 0.");

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("Cliente é obrigatório.")
                .GreaterThan(0).WithMessage("Número do Pedido deve ser maior que 0.");
        }
    }
}
