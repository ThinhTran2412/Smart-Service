using Microsoft.AspNetCore.Mvc;
using SmartService.Application.UseCases.AnalyzeServiceRequest;

namespace SmartService.API.Controllers;

[ApiController]
[Route("api/service-analysis")]
public class ServiceAnalysisController : ControllerBase
{
    private readonly AnalyzeServiceRequestHandler _handler;

    public ServiceAnalysisController(AnalyzeServiceRequestHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Analyze([FromBody] string description)
    {
        if (string.IsNullOrEmpty(description))
            return BadRequest(new { error = "Description is required" });

        var complexity = await _handler.HandleAsync(description);
        return Ok(new { complexity = complexity.Level });
    }
}

