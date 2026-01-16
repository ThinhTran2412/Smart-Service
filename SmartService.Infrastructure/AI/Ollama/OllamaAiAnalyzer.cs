using SmartService.Application.Abstractions.AI;
using SmartService.Application.DTOs;
using SmartService.Infrastructure.KnowledgeBase.Complexity;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartService.Infrastructure.AI.Ollama;

public sealed class OllamaAiAnalyzer : IAiAnalyzer
{
    private readonly OllamaClient _client;
    private readonly ComplexityRuleProvider _rules;

    public OllamaAiAnalyzer(
        OllamaClient client,
        ComplexityRuleProvider rules)
    {
        _client = client;
        _rules = rules;
    }

    public async Task<AiAnalysisResultDto> AnalyzeAsync(
        string description,
        CancellationToken cancellationToken = default)
    {
        var rule = _rules.GetRule("TECH_ELECTRIC");
        var ruleJson = JsonSerializer.Serialize(rule);

        var prompt = OllamaPromptBuilder.BuildComplexityPrompt(description, ruleJson);

        var rawResponse = await _client.GenerateAsync(
            "yasserrmd/Qwen2.5-7B-Instruct-1M",
            prompt);

        // Parse JSON response từ Ollama
        var complexityLevel = ExtractComplexityLevel(rawResponse);

        return new AiAnalysisResultDto
        {
            ServiceGroup = "TECH",
            SubCategoryId = "TECH_ELECTRIC",
            CaseId = Guid.NewGuid().ToString(),
            ComplexityLevel = complexityLevel,
            Confidence = 0.9,
            SafetyRisk = complexityLevel >= 4,
            LegalRequired = false
        };
    }

    private static int ExtractComplexityLevel(string jsonResponse)
    {
        if (string.IsNullOrWhiteSpace(jsonResponse))
            return 1;

        try
        {
            // Try to parse full JSON response
            var response = JsonSerializer.Deserialize<ComplexityResponse>(jsonResponse);
            if (response?.ComplexityLevel > 0 && response.ComplexityLevel <= 5)
                return response.ComplexityLevel;
        }
        catch
        {
            // Continue to fallback parsing
        }

        try
        {
            // Try to extract JSON object if not valid at root level
            var jsonMatch = System.Text.RegularExpressions.Regex.Match(
                jsonResponse,
                @"\{[^{}]*""complexityLevel""\s*:\s*(\d+)[^{}]*\}");
            
            if (jsonMatch.Success && int.TryParse(jsonMatch.Groups[1].Value, out var level))
            {
                if (level >= 1 && level <= 5)
                    return level;
            }
        }
        catch
        {
            // Continue to number extraction
        }

        // Try to extract just the number from anywhere in the response
        var numberMatch = System.Text.RegularExpressions.Regex.Match(
            jsonResponse,
            @"[^0-9]*([1-5])[^0-9]*");
        
        if (numberMatch.Success && int.TryParse(numberMatch.Groups[1].Value, out var extractedLevel))
        {
            return extractedLevel;
        }

        // Default fallback - return 1 for invalid responses
        return 1;
    }

    private sealed class ComplexityResponse
    {
        [JsonPropertyName("complexityLevel")]
        public int ComplexityLevel { get; set; }
    }
}

