using Epsilon.Abstractions.Components;
using Epsilon.Host.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : ControllerBase
{
    private readonly IPageComponentManager _pageComponentManager;

    public ComponentController(IPageComponentManager pageComponentManager)
    {
        _pageComponentManager = pageComponentManager;
    }

    [HttpGet("page/{pageName}")]
    public async Task<ActionResult<PageComponent>> GetPage(int courseId, string pageName)
    {
        var pageComponent = await _pageComponentManager.Fetch(courseId, pageName);

        return Ok(pageComponent);
    }

    [HttpPost("page/{pageName}")]
    public async Task<ActionResult<PageComponent>> UpdateOrCreatePage(int courseId, string pageName, [FromBody] PageUpdateRequest updateRequest)
    {
        var createdPageComponent = await _pageComponentManager.CreateOrUpdate(courseId, pageName, updateRequest.Body);

        return Ok(createdPageComponent);
    }
}