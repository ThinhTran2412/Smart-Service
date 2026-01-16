using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartService.Application.Abstractions.AI;
using SmartService.Infrastructure.AI.Ollama;
using SmartService.Infrastructure.KnowledgeBase.Complexity;
using SmartService.Infrastructure.Persistence;

namespace SmartService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContextFactory<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));


        services.AddSingleton<ComplexityRuleProvider>();
        services.AddSingleton<SimpleComplexityMatcher>();
        services.AddScoped<IAiAnalyzer, OllamaAiAnalyzer>();


        

        return services;
    }
}

