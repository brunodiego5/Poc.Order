using FluentValidation;

namespace Poc.Order.Api.Application.Queries.GetOnePedido
{
    public class GetOnePedidoQueryValidator : AbstractValidator<GetOnePedidoQuery>
    {
        public GetOnePedidoQueryValidator()
        {
            RuleFor(x => x.PedidoId)
                .NotEmpty().WithMessage("Número do Pedido é obrigatório.")
                .GreaterThan(0).WithMessage("Número do Pedido deve ser maior que 0.");
        }
    }
}
