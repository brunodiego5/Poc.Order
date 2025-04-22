using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Poc.Order.Api.Application.Commands.CreatePedido;
using Poc.Order.Api.Application.Commands.UpdateImpostoPedido;
using Poc.Order.Api.Application.Profiles;
using Poc.Order.Api.Application.Queries.GetOnePedido;
using Poc.Order.Api.Application.Queries.GetPedidos;
using Poc.Order.Api.Domain.Interfaces;
using Poc.Order.Api.Infrastructure.Data.Profiles;
using Poc.Order.Api.Infrastructure.Data.Repositories;
using Poc.Order.Api.Infrastructure.Publisher.Profiles;
using Poc.Order.Api.Infrastructure.Publisher.Publishers;
using RabbitMQ.Client;

namespace Poc.Order.Api.IoC
{
    public static class Bootstrapper
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterData(services, configuration);
            RegisterRepositories(services);
            RegisterValidators(services);
            RegisterMappers(services);
            RegisterMediatRs(services);
            RegisterBus(services, configuration);
            RegisterPublishers(services);
        }

        private static void RegisterData(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(_ =>
                new MongoClient(configuration["MongoDb:ConnectionString"]));
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IPedidoRepository, PedidoRepository>();
        }

        private static void RegisterValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<CreatePedidoCommand>, CreatePedidoCommandValidator>();
            services.AddScoped<IValidator<GetOnePedidoQuery>, GetOnePedidoQueryValidator>();
            services.AddScoped<IValidator<GetPedidosQuery>, GetPedidosQueryValidator>();
            services.AddScoped<IValidator<UpdateImpostoPedidoCommand>, UpdateImpostoPedidoCommandValidator>();
        }

        private static void RegisterMappers(IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(CreatePedidoCommandToPedidoProfile).Assembly,
                typeof(PedidoToPedidoModelProfile).Assembly,
                typeof(PedidoToPedidoMessageProfile).Assembly
            );
        }

        private static void RegisterMediatRs(IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreatePedidoCommandHandler).Assembly);
            });
        }

        private static void RegisterBus(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = configuration["RabbitMq:Host"],
                    Port = int.Parse(configuration["RabbitMq:Port"]),
                    UserName = configuration["RabbitMq:User"],
                    Password = configuration["RabbitMq:Password"],
                    VirtualHost = configuration["RabbitMq:VirtualHost"]

                };

                return factory.CreateConnection();
            });

        }

        private static void RegisterPublishers(IServiceCollection services)
        {
            services.AddScoped<IPedidoPublisher, PedidoPublisher>();
        }
    }
}
