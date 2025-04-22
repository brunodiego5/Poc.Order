using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Order.Api.IoC;
using Poc.Order.Processor.Workers;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.Sources.Clear();
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args);
    })
    .ConfigureServices((context, services) =>
    {
        services.RegisterServices(context.Configuration);
        services.AddHostedService<Worker>();
    })
    .UseSerilog((context, services, config) =>
    {
        config.ReadFrom.Configuration(context.Configuration);
    })
    .Build();

await host.RunAsync();