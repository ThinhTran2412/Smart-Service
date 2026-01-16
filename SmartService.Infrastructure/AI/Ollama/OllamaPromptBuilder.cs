namespace SmartService.Infrastructure.AI.Ollama;

public static class OllamaPromptBuilder
{
    public static string BuildComplexityPrompt(
        string description,
        string ruleJson)
    {
        return $$"""
        You are an expert service complexity analyzer. Analyze the given service description 
        and determine its complexity level based on the provided rules.

        The complexity levels are defined by criteria that indicate severity and technical difficulty.
        You must understand the semantic meaning of the description, not just match exact phrases.
        For example, if someone says "electrical fire hazard" you should recognize it relates to 
        "Nguy cơ cháy nổ" (fire/explosion risk).

        Rules (JSON):
        {{ruleJson}}

        Service Description:
        {{description}}

        Instructions:
        1. Analyze the description semantically, not just by exact text matching
        2. Consider the technical complexity and risk level implied
        3. Match to the most appropriate complexity level (1-5)
        4. Higher numbers = higher complexity/risk
        5. Return ONLY valid JSON with no additional text

        Output format:
        {
          "complexityLevel": <number 1-5>
        }
        """;
    }
}
