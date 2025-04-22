using FluentValidation;

namespace Poc.Order.Api.Application.Commands.UpdateImpostoPedido
{
    public class UpdateImpostoPedidoCommandValidator : AbstractValidator<UpdateImpostoPedidoCommand>
    {
        public UpdateImpostoPedidoCommandValidator()
        {
            RuleFor(x => x.PedidoId)
                .NotEmpty().WithMessage("Número do Pedido é obrigatório.")
                .GreaterThan(0).WithMessage("Número do Pedido deve ser maior que 0.");

            RuleFor(x => x.Imposto)
                .NotEmpty().WithMessage("Valor do Imposto é obrigatório.")
                .GreaterThanOrEqualTo(0).WithMessage("Valor do Imposto não pode ser negativo.");
        }
    }
}
