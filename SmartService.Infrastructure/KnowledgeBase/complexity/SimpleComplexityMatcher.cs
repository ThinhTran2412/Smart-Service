using SmartService.Domain.Knowledge.Complexity;

namespace SmartService.Infrastructure.KnowledgeBase.Complexity;

public sealed class SimpleComplexityMatcher
{
    public int MatchComplexity(string description, ComplexityRule rule)
    {
        var descriptionLower = description.ToLower().Trim();
        
        // Search from highest level to lowest for best match
        foreach (var level in rule.Levels.OrderByDescending(l => l.Level))
        {
            // Check if any criteria phrase appears in the description
            foreach (var criterion in level.Criteria)
            {
                if (descriptionLower.Contains(criterion.ToLower()))
                {
                    return level.Level;
                }
            }
        }
        
        // Default to level 1 if no match found
        return 1;
    }
}
