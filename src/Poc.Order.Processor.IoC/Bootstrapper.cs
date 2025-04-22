using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Poc.Order.Processor.Application.Commands.CalcularPedido;
using Poc.Order.Processor.Application.Profiles;
using Poc.Order.Processor.Domain.DomainServices;
using Poc.Order.Processor.Domain.DomainServices.Strategies;
using Poc.Order.Processor.Domain.Interfaces;
using Poc.Order.Processor.Service.Profiles;
using Poc.Order.Processor.Service.ServiceHandlers;
using Poc.Order.Processor.Subscriber.Profiles;
using Poc.Order.Processor.Subscriber.Subscribers;
using RabbitMQ.Client;

namespace Poc.Order.Api.IoC
{
    public static class Bootstrapper
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterMappers(services);
            RegisterMediatRs(services);
            RegisterBus(services, configuration);
            RegisterSubscribers(services);
            RegisterExtensions(services);
            RegisterDomainServices(services);
            RegisterServiceHandlers(services);
        }

        private static void RegisterMappers(IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(PedidoMessageToCalcularPedidoCommandProfile).Assembly,
                typeof(PedidoToEnviarImpostoRequestProfile).Assembly,
                typeof(CalcularPedidoCommandToPedidoProfile).Assembly
            );
        }

        private static void RegisterMediatRs(IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CalcularPedidoCommandHandler).Assembly);
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

        private static void RegisterSubscribers(IServiceCollection services)
        {
            services.AddScoped<IPedidoSubscriber, PedidoSubscriber>();
        }

        private static void RegisterExtensions(IServiceCollection services)
        {
            services.AddFeatureManagement();
            services.AddHttpClient();
        }

        private static void RegisterDomainServices(IServiceCollection services)
        {
            services.AddScoped<IPedidoImpostoStrategy, ImpostoAtualStrategy>();
            services.AddScoped<IPedidoImpostoStrategy, ImpostoReformaStrategy>();
            services.AddScoped<IPedidoDomainService, PedidoDomainService>();
        }

        private static void RegisterServiceHandlers(IServiceCollection services)
        {
            services.AddScoped<IPedidoServiceHandler, PedidoServiceHandler>();
        }
    }
}
