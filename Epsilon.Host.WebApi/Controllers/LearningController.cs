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
        var outcomes = await _learningOutcomeCanvasResultService.GetSubmissions(studentId).ToListAsync();

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
    
    [HttpGet("domain/outcomes")]
    public async Task<ActionResult<LearningDomainOutcome>> GetDomainOutcomes()
    {
        var domainOutcomes = await _learningDomainService.GetOutcomes();

        return Ok(domainOutcomes);
    }
}