using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace SupportCaseManagement.Module.Services
{
    public class AIService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public AIService(IConfiguration configuration)
        {
            this.configuration = configuration;
            httpClient = new HttpClient();
        }

        public async Task<string> CallAIAsync(string prompt)
        {
            var apiKey = configuration["OpenAI:ApiKey"];

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "You are a support case assistant." },
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);

            var response = await httpClient.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            var responseContent = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseContent);

            var result = doc
                .RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return result ?? "No AI response.";
        }
    }
}