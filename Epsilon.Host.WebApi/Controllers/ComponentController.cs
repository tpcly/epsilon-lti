using Epsilon.Abstractions.Service;
using Epsilon.Canvas.Abstractions.Rest;
using Epsilon.Host.WebApi.Models;
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

    [HttpGet("page/{pageName}")]
    public async Task<ActionResult<Page>> GetPage(int courseId, string pageName)
    {
        var page = await _pageEndpoint.GetPage(courseId, pageName);

        if (page == null)
        {
            return NotFound();
        }

        return Ok(page);
    }

    [HttpPost("page/{pageName}")]
    public async Task<ActionResult<Page>> UpdateOrCreatePage(int courseId, string pageName, [FromBody] PageUpdateRequest updateRequest)
    {
        var page = await _pageEndpoint.UpdateOrCreatePage(courseId, new Page(pageName) { Title = pageName, Body = updateRequest.Body, });

        if (page == null)
        {
            return NotFound();
        }

        return Ok(page);
    }
}