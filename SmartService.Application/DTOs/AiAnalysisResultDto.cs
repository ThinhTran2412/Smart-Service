namespace SmartService.Application.DTOs;

public class AiAnalysisResultDto
{
    public string ServiceGroup { get; set; } = default!;
    public string SubCategoryId { get; set; } = default!;
    public string CaseId { get; set; } = default!;

    public int ComplexityLevel { get; set; }
    public double Confidence { get; set; }

    public bool SafetyRisk { get; set; }
    public bool LegalRequired { get; set; }
}
