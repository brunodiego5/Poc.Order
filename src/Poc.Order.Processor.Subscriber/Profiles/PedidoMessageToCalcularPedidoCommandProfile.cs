using AutoMapper;
using Poc.Order.Processor.Application.Commands.CalcularPedido;
using Poc.Order.Processor.Infrastructure.Publisher.Messages;

namespace Poc.Order.Processor.Subscriber.Profiles
{
    public class PedidoMessageToCalcularPedidoCommandProfile : Profile
    {
        public PedidoMessageToCalcularPedidoCommandProfile()
        {
            CreateMap<PedidoMessage, CalcularPedidoCommand>()
                .ForMember(dest => dest.CorrelationId, opt => opt.MapFrom(src => src.CorrelationId))
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.PedidoId))
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens));

            CreateMap<ItemPedidoMessage, ItemPedidoCommand>()
                .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.Quantidade))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor));
        }
    }
}
