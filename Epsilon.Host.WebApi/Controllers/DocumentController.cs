using Epsilon.Abstractions.Components;
using Epsilon.Abstractions.Services;
using Epsilon.Host.WebApi.Models;
using Epsilon.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly IPageComponentManager _pageComponentManager;
    private readonly ICompetenceDocumentService _competenceDocumentService;

    public DocumentController(IPageComponentManager pageComponentManager, ICompetenceDocumentService competenceDocumentService)
    {
        _pageComponentManager = pageComponentManager;
        _competenceDocumentService = competenceDocumentService;
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


    [HttpGet("download/word")]
    public async Task<IActionResult> DownloadWord(string userId, DateTime from, DateTime to)
    {
        var document = _competenceDocumentService.GetDocument(userId, from, to);

        using var stream = new MemoryStream();
        await _competenceDocumentService.WriteDocument(stream, document);

        return File(
            stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "CompetenceDocument.docx"
        );
    }
}