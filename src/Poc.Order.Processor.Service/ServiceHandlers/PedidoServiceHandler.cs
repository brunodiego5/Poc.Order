using AutoMapper;
using Microsoft.Extensions.Configuration;
using Poc.Order.Processor.Domain.Entities;
using Poc.Order.Processor.Domain.Enums;
using Poc.Order.Processor.Domain.Interfaces;
using Poc.Order.Processor.Service.Contracts;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Poc.Order.Processor.Service.ServiceHandlers
{
    public class PedidoServiceHandler : IPedidoServiceHandler
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public PedidoServiceHandler(IHttpClientFactory httpClientFactory, IMapper mapper, IConfiguration configuration)
        {
            this.mapper = mapper;
            var baseUrl = configuration["ServicosExternos:Pedidos:BaseUrl"];
            this.httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<bool> EnviarImposto(Pedido pedido, string correlationId, CancellationToken cancellationToken)
        {
            var enviarPedidoRequest = mapper.Map<EnviarImpostoRequest>(pedido);
            enviarPedidoRequest.CorrelationId = correlationId;

            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/pedido")
            {
                Content = new StringContent(JsonSerializer.Serialize(enviarPedidoRequest), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var enviarPedidoResponse = await response.Content.ReadFromJsonAsync<EnviarImpostoResponse>(cancellationToken);

            return await Task.FromResult(enviarPedidoResponse.Status == StatusPedido.Concluido);
        }
    }
}
