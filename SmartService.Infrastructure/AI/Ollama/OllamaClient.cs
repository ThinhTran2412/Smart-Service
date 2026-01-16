using System.Net.Http.Json;

namespace SmartService.Infrastructure.AI.Ollama;

public sealed class OllamaClient
{
    private readonly HttpClient _http;

    public OllamaClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> GenerateAsync(string model, string prompt)
    {
        var response = await _http.PostAsJsonAsync(
            "http://localhost:11434/api/chat",
            new
            {
                model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                stream = false
            });

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<OllamaChatResponse>();

        return result?.Message?.Content ?? string.Empty;
    }

    private sealed class OllamaChatResponse
    {
        public ChatMessage? Message { get; set; }
    }

    private sealed class ChatMessage
    {
        public string Content { get; set; } = string.Empty;
    }
}
