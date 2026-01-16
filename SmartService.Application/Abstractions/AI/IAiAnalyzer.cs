namespace SmartService.Application.Abstractions.AI;

public interface IAiAnalyzer
{
    Task<SmartService.Application.DTOs.AiAnalysisResultDto> AnalyzeAsync(
        string description,
        CancellationToken cancellationToken = default);
}
