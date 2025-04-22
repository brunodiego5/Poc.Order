using AutoMapper;
using Poc.Order.Api.Domain.Entities;
using Poc.Order.Api.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Order.Api.Infrastructure.Data.Profiles
{
    public class PedidoToPedidoModelProfile : Profile
    {
        public PedidoToPedidoModelProfile()
        {
            CreateMap<Pedido, PedidoModel>()
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.PedidoId))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens))
                .ForMember(dest => dest.Imposto, opt => opt.MapFrom(src => src.Imposto))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ItemPedido, ItemPedidoModel>()
                .ReverseMap();
        }
    }
}
