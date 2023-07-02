using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("learning-domain")]
public class LearningDomainController : ControllerBase
{
    private readonly ILearningDomainService _learningDomainService;

    public LearningDomainController(ILearningDomainService learningDomainService)
    {
        _learningDomainService = learningDomainService;
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<LearningDomain>> GetName(string name)
    {
        var domain = await _learningDomainService.GetDomain(name);
        if (domain == null)
        {
            return NotFound();
        }

        return Ok(domain);
    }
}