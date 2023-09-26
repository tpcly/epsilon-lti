using Epsilon.Abstractions.Components;
using Epsilon.Abstractions.Services;
using Epsilon.Host.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
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
    public async Task<ActionResult<PageComponent>> GetPage(int courseId, string pageName, string userId, string name)
    {
        var pageComponent = await _pageComponentManager.Fetch(courseId, pageName, userId, name);

        return Ok(pageComponent);
    }

    [HttpPost("page/{pageName}")]
    public async Task<ActionResult<PageComponent>> UpdateOrCreatePage(int courseId, string pageName, [FromBody] PageUpdateRequest updateRequest)
    {
        var createdPageComponent = await _pageComponentManager.CreateOrUpdate(courseId, pageName, updateRequest.Body);

        return Ok(createdPageComponent);
    }


    [HttpGet("download/word")]
    public async Task<IActionResult> DownloadWord(int courseId, string userId, DateTime from, DateTime to)
    {
        var document = await _competenceDocumentService.GetDocument(courseId, userId, from, to);

        var stream = new MemoryStream();
        _competenceDocumentService.WriteDocument(stream, document);

        return File(
            stream,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "CompetenceDocument.docx"
        );
    }
}