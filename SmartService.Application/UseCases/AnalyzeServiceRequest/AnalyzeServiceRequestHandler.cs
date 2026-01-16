using SmartService.Application.Abstractions.AI;
using SmartService.Domain.ValueObjects;

namespace SmartService.Application.UseCases.AnalyzeServiceRequest;

public sealed class AnalyzeServiceRequestHandler
{
    private readonly IAiAnalyzer _ai;

    public AnalyzeServiceRequestHandler(IAiAnalyzer ai)
    {
        _ai = ai;
    }

   public async Task<ServiceComplexity> HandleAsync(string description)
{
    var aiResult = await _ai.AnalyzeAsync(description);

    // 1️⃣ Lấy giá trị AI trả về
    var aiLevel = aiResult?.ComplexityLevel ?? 3;

    // 2️⃣ Normalize / khóa mức độ phức tạp
    var normalizedLevel = aiLevel switch
    {
        <= 1 => 1,
        2 => 2,
        3 => 3,
        4 => 4,
        >= 5 => 5
    };

    // 3️⃣ Đưa vào Domain (luôn hợp lệ)
    return ServiceComplexity.From(normalizedLevel);
}

}
