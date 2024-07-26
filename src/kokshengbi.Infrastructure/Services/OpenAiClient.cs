using kokshengbi.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace kokshengbi.Infrastructure.Services
{
    public class OpenAiClient : IOpenAiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];
        }

        public async Task<string> GenerateTextAsync(string prompt)
        {
            var requestBody = new
            {
                prompt = prompt,
                max_tokens = 100,
                model = "text-ada-001"
            };

            var jsonRequestBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/engines/ada/completions", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonDocument.Parse(responseContent);
            return responseJson.RootElement.GetProperty("choices")[0].GetProperty("text").GetString();
        }
    }
}
