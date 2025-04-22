using AutoMapper;
using Poc.Order.Processor.Domain.Entities;
using Poc.Order.Processor.Service.Contracts;

namespace Poc.Order.Processor.Service.Profiles
{
    public class PedidoToEnviarImpostoRequestProfile : Profile
    {
        public PedidoToEnviarImpostoRequestProfile()
        {
            CreateMap<Pedido, EnviarImpostoRequest>()
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.PedidoId))
                .ForMember(dest => dest.Imposto, opt => opt.MapFrom(src => src.Imposto))
                .ForMember(dest => dest.CorrelationId, opt => opt.Ignore());
        }
    }
}
