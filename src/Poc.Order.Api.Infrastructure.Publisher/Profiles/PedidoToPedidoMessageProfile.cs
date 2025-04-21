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
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens))
                .ForMember(dest => dest.CorrelationId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ItemPedido, ItemPedidoMessage>()
                .ReverseMap();
        }
    }
}
