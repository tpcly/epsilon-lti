using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class LearningController : ControllerBase
{
    private readonly ILearningDomainService _learningDomainService;
    private readonly ILearningOutcomeCanvasResultService _learningOutcomeCanvasResultService;
    private readonly IAuthorizationUser _authorizationUser;


    public LearningController(ILearningDomainService learningDomainService, ILearningOutcomeCanvasResultService learningOutcomeCanvasResultService, IAuthorizationUser authorizationUser)
    {
        _learningDomainService = learningDomainService;
        _learningOutcomeCanvasResultService = learningOutcomeCanvasResultService;
        _authorizationUser = authorizationUser;
    }

    [HttpGet("outcomes")]
    public async Task<ActionResult<IAsyncEnumerable<LearningDomainSubmission>>> GetResults(string studentId)
    {
        if (await _authorizationUser.HasCurrentUserAccessToUser(studentId))
        {
            var outcomes = await _learningOutcomeCanvasResultService.GetSubmissions(studentId).ToListAsync();

            return Ok(outcomes);
        }

        return Forbid();
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