using System.Text.Json.Serialization;

namespace SmartService.Domain.Knowledge.Complexity;

public class ComplexityLevelRule
{
    [JsonPropertyName("level")]
    public int Level { get; set; }
    
    [JsonPropertyName("criteria")]
    public List<string> Criteria { get; set; } = new();
}
