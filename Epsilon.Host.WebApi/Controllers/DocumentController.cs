using Epsilon.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly ICompetenceDocumentService _competenceDocumentService;

    public DocumentController(ICompetenceDocumentService competenceDocumentService)
    {
        _competenceDocumentService = competenceDocumentService;
    }


    [HttpGet("download/word")]
    public async Task<IActionResult> DownloadWord(string userId, DateTime from, DateTime to, string domains)
    {
        var document = _competenceDocumentService.GetDocument(userId, from, to, domains.Split(','));

        using var stream = new MemoryStream();
        await _competenceDocumentService.WriteDocument(stream, await document);

        return File(
            stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "CompetenceDocument.docx"
        );
    }
}