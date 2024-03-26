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
    public async Task<IActionResult> DownloadWord(string userId, DateTime from, DateTime to)
    {
        var document = _competenceDocumentService.GetDocument(userId, from, to);

        using var stream = new MemoryStream();
        await _competenceDocumentService.WriteDocument(stream, await document);

        return File(
            stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "CompetenceDocument.docx"
        );
    }
    
    
    [HttpGet("download/csv")]
    public async Task<IActionResult> DownloadCsv(List<string> userIds, DateTime from, DateTime to)
    {
        // var document = _competenceDocumentService.GetDocument(userId, from, to);
        //
        // using var stream = new MemoryStream();
        // await _competenceDocumentService.WriteDocument(stream, await document);

        // return File(
        //     stream.ToArray(),
        //     "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        //     "CompetenceDocument.docx"
        // );
    }
}