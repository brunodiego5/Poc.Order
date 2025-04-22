using AutoMapper;
using Poc.Order.Api.Application.Queries.GetPedidos;
using Poc.Order.Api.Domain.Entities;

namespace Poc.Order.Api.Application.Profiles
{
    public class PedidoToGetPedidosQueryResponseyProfile : Profile
    {
        public PedidoToGetPedidosQueryResponseyProfile()
        {
            CreateMap<Pedido, GetPedidosQueryResponse>()
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.PedidoId))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens))
                .ForMember(dest => dest.Imposto, opt => opt.MapFrom(src => src.Imposto))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<ItemPedido, ItemPedidosQuery>();
        }
    }
}
