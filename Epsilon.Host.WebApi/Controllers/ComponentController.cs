using Epsilon.Abstractions.Service;
using Epsilon.Canvas.Abstractions.Rest;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : ControllerBase
{
    private readonly IPageEndpoint _pageEndpoint;

    public ComponentController(IPageEndpoint pageEndpoint)
    {
        _pageEndpoint = pageEndpoint;
    }
    
    [HttpGet("Page/{pageName}")]
    public async Task<ActionResult<Page>> GetPage(string pageName, int courseId)
    {
        var page = await _pageEndpoint.GetPage(courseId, pageName);

        if (page == null)
        {
            return NotFound();
        }

        return Ok(page);
    }
    
    [HttpPost("Page/persona")]
    public async Task<ActionResult<Page>> UpdatePersona(int courseId, string html)
    {
        var page = await _pageEndpoint.UpdateOrCreatePage(courseId, "persona", html);

        if (page == null)
        {
            return NotFound();
        }

        return Ok(page);
    }
    
    [HttpPost("Page/reflection")]
    public async Task<ActionResult<Page>> UpdateReflection(int courseId, string html)
    {
        var page = await _pageEndpoint.UpdateOrCreatePage(courseId, "reflection", html);

        if (page == null)
        {
            return NotFound();
        }

        return Ok(page);
    }
}