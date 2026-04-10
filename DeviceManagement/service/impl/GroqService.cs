using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DeviceManagement.service
{
    public class GroqService : IGroqService
    {
        private const string GroqEndpoint = "https://api.groq.com/openai/v1/chat/completions";
        private const string DefaultModel = "llama-3.3-70b-versatile";

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GroqService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        /// <summary>
        /// Sends a request to the API and retrieves the response based on the provided prompt
        /// </summary>
        /// <param name="prompt">User input</param>
        /// <param name="cancellationToken">Token used to cancel the request</param>
        /// <returns>API response</returns>
        public async Task<string> SendPromptAsync(string prompt, CancellationToken cancellationToken = default)
        {
            var apiKey = _configuration["GROQ_API_KEY"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("GROQ_API_KEY is not configured.");
            }

            var payload = new
            {
                model = DefaultModel,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, GroqEndpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Groq API returned {(int)response.StatusCode}: {responseBody}");
            }

            using var jsonDoc = JsonDocument.Parse(responseBody);
            var content = jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return content ?? string.Empty;
        }
    }
}
