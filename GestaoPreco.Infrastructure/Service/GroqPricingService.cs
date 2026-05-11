using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using GestaoPedido.Infrastructure.Interface;
using Microsoft.Extensions.Configuration;

namespace GestaoPedido.Infrastructure.Service;

public class GroqPricingService : IAiPricingService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GroqPricingService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> AnalyzePrice(string product, decimal currentPrice)
    {
        var apiKey = _configuration["Groq:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException(
                "Configure Groq:ApiKey (ex.: User Secrets ou variável de ambiente).");

        var model = _configuration["Groq:Model"] ?? "llama-3.3-70b-versatile";
        var prompt =
            $"Analise o preço do produto {product} no valor de {currentPrice} reais e sugira melhorias.";

        var body = new
        {
            model,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var json = JsonSerializer.Serialize(
            body,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        using var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(
                $"Groq API {(int)response.StatusCode} {response.ReasonPhrase}: {responseContent}");

        return responseContent;
    }
}
