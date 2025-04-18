using AutoMapper;
using Poc.Order.Api.Application.Commands.CreatePedido;
using Poc.Order.Api.Domain.Entities;

namespace Poc.Order.Api.Application.Profiles
{
    public class CreatePedidoCommandToPedidoProfile : Profile
    {
        public CreatePedidoCommandToPedidoProfile()
        {
            CreateMap<CreatePedidoCommand, Pedido>()
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.PedidoId))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens))
                .ForMember(dest => dest.Imposto, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<ItemPedidoCommand, ItemPedido>();
        }
    }
}
