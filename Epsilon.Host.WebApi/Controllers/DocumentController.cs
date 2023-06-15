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
    public async Task<IActionResult> DownloadWord(int courseId, DateTime from, DateTime to)
    {
        var document = await _competenceDocumentService.GetDocument(courseId, from, to);

        var stream = new MemoryStream();
        await _competenceDocumentService.WriteDocument(stream, document);

        return File(
            stream,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "CompetenceDocument.docx"
        );
    }
}