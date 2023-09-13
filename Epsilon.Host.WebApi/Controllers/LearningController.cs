using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LearningController : ControllerBase
{
    private readonly ILearningDomainService _learningDomainService;
    private readonly ILearningOutcomeCanvasResultService _learningOutcomeCanvasResultService;


    public LearningController(ILearningDomainService learningDomainService, ILearningOutcomeCanvasResultService learningOutcomeCanvasResultService)
    {
        _learningDomainService = learningDomainService;
        _learningOutcomeCanvasResultService = learningOutcomeCanvasResultService;
    }

    [HttpGet("outcomes")]
    public async Task<ActionResult<IAsyncEnumerable<LearningDomainSubmission>>> GetResults(string studentId)
    {
        var outcomes = _learningOutcomeCanvasResultService.GetSubmissions(studentId);

        return Ok(outcomes);
    }

    [HttpGet("domain/{name}")]
    public async Task<ActionResult<LearningDomain>> GetDomain(string name)
    {
        var domain = await _learningDomainService.GetDomain(name);
        if (domain == null)
        {
            return NotFound();
        }

        return Ok(domain);
    }
    
    [HttpGet("domain/{tenantId}/outcomes")]
    public async Task<ActionResult<LearningDomainOutcome>> GetDomainOutcomes(string tenantId)
    {
        var domainOutcomes = await _learningDomainService.GetOutcomes();

        return Ok(domainOutcomes);
    }
}