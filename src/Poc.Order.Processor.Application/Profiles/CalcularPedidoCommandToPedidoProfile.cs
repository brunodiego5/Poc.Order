using AutoMapper;
using Poc.Order.Processor.Application.Commands.CalcularPedido;
using Poc.Order.Processor.Domain.Entities;

namespace Poc.Order.Processor.Application.Profiles
{
    public class CalcularPedidoCommandToPedidoProfile : Profile
    {
        public CalcularPedidoCommandToPedidoProfile()
        {
            CreateMap<CalcularPedidoCommand, Pedido>()
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.PedidoId))
                .ForMember(dest => dest.Imposto, opt => opt.Ignore())
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens));

            CreateMap<ItemPedidoCommand, ItemPedido>()
                .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.Quantidade))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor));
        }
    }
}
