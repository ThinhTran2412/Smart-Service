using Microsoft.Extensions.Hosting;
using System.Text.Json;
using SmartService.Domain.Knowledge.Complexity;

namespace SmartService.Infrastructure.KnowledgeBase.Complexity;

public class ComplexityRuleProvider
{
    private readonly Dictionary<string, ComplexityRule> _rules = new();

    public ComplexityRuleProvider(IHostEnvironment env)
    {
        LoadRule(
            env,
            "TECH_ELECTRIC",
            "KnowledgeBase/complexity/technical/electric_complexity.json"
        );
    }

    private void LoadRule(IHostEnvironment env, string key, string relativePath)
    {
        var fullPath = Path.Combine(env.ContentRootPath, relativePath);
        
        // Nếu file không tìm thấy trong ContentRootPath, thử tìm trong thư mục bin output
        if (!File.Exists(fullPath))
        {
            var binPath = Path.Combine(env.ContentRootPath, "bin", "Debug", "net8.0", relativePath);
            if (File.Exists(binPath))
            {
                fullPath = binPath;
            }
            else
            {
                // Thử thư mục Release
                binPath = Path.Combine(env.ContentRootPath, "bin", "Release", "net8.0", relativePath);
                if (File.Exists(binPath))
                {
                    fullPath = binPath;
                }
            }
        }

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Rule file not found: {fullPath}");

        var json = File.ReadAllText(fullPath);
        var rule = JsonSerializer.Deserialize<ComplexityRule>(json)!;

        _rules[key] = rule;
    }

    public ComplexityRule GetRule(string key)
    {
        return _rules[key];
    }

}
