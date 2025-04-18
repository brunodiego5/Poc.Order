using AutoMapper;
using Poc.Order.Api.Domain.Entities;
using Poc.Order.Api.Infrastructure.Publisher.Messages;

namespace Poc.Order.Api.Infrastructure.Publisher.Profiles
{
    public class PedidoToPedidoMessageProfile : Profile
    {
        public PedidoToPedidoMessageProfile()
        {
            CreateMap<Pedido, PedidoMessage>()
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.PedidoId))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens))
                .ForMember(dest => dest.Imposto, opt => opt.MapFrom(src => src.Imposto))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CorrelationId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ItemPedido, ItemPedidoMessage>()
                .ReverseMap();
        }
    }
}
