using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Poc.Order.Api.Application.Commands.CreatePedido;
using Poc.Order.Api.Application.Profiles;
using Poc.Order.Api.Domain.Interfaces;
using Poc.Order.Api.Infrastructure.Data.Profiles;
using Poc.Order.Api.Infrastructure.Data.Repositories;

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
        }

        private static void RegisterData(IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine(configuration["MongoDb:ConnectionString"]);
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
        }

        private static void RegisterMappers(IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(CreatePedidoCommandToPedidoProfile).Assembly,
                typeof(PedidoToPedidoModelProfile).Assembly
            );
        }

        private static void RegisterMediatRs(IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreatePedidoCommandHandler).Assembly);
            });
        }
    }
}
