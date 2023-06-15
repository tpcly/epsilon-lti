using Epsilon.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : Controller
{
    private readonly ICompetenceDocumentService _competenceDocumentService;

    public DocumentController(ICompetenceDocumentService competenceDocumentService)
    {
        _competenceDocumentService = competenceDocumentService;
    }

    [HttpGet("word")]
    public async Task<IActionResult> DownloadWord(DateTime from, DateTime to)
    {
        var stream = new MemoryStream();
        await _competenceDocumentService.WriteDocument(stream, from, to);

        return File(
            stream,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "CompetenceDocument.docx"
        );
    }
}