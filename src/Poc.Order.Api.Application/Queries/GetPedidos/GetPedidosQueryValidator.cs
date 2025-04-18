using FluentValidation;

namespace Poc.Order.Api.Application.Queries.GetPedidos
{
    public class GetPedidosQueryValidator : AbstractValidator<GetPedidosQuery>
    {
        public GetPedidosQueryValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status do Pedido é obrigatório.");
        }
    }
}
