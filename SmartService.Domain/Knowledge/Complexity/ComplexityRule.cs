using System.Text.Json.Serialization;

namespace SmartService.Domain.Knowledge.Complexity;

public class ComplexityRule
{
    [JsonPropertyName("subCategoryId")]
    public string SubCategoryId { get; set; } = string.Empty;
    
    [JsonPropertyName("levels")]
    public List<ComplexityLevelRule> Levels { get; set; } = new();
}
