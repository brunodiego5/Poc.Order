using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Poc.Order.Api.Domain.Entities;
using Poc.Order.Api.Domain.Interfaces;
using Poc.Order.Api.Infrastructure.Data.Models;

namespace Poc.Order.Api.Infrastructure.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly IMongoCollection<PedidoModel> collection;
        private readonly IMapper mapper;

        public PedidoRepository(IMongoClient client, IConfiguration configuration, IMapper mapper)
        {
            Console.WriteLine(configuration["MongoDb:Database"]);
            var database = client.GetDatabase(configuration["MongoDb:Database"]);
            collection = database.GetCollection<PedidoModel>("pedidos");

            this.mapper = mapper;
        }

        public async Task AddPedidoAsync(Pedido pedido, CancellationToken cancellationToken)
        {
            var pedidoModel = mapper.Map<PedidoModel>(pedido);
            await collection.InsertOneAsync(pedidoModel);
        }

        public async Task<Pedido> GetPedidoByIdAsync(int id, CancellationToken cancellationToken)
        {
            var pedidoModel = await collection.Find(w => w.PedidoId == id).FirstOrDefaultAsync();

            return mapper.Map<Pedido>(pedidoModel);
        }
    }
}
