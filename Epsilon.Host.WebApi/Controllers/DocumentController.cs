using System.Collections.ObjectModel;
using Epsilon.Abstractions;
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
    private readonly IEduBadgeService _eduBadgeService;

    public DocumentController(ICompetenceDocumentService competenceDocumentService, IEduBadgeService eduBadgeService)
    {
        _competenceDocumentService = competenceDocumentService;
        _eduBadgeService = eduBadgeService;
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
    
    
    [HttpPost("download/csv")]
    public async Task<List<LearningDomainSubmission>> DownloadCsv(Collection<string> userIds, DateTime from, DateTime to)
    {
        var data = await _eduBadgeService.GetData(userIds, from, to);
        return data;
        // using var stream = new MemoryStream();
        // _eduBadgeService.WriteDocument(stream, data);
        //
        // return File(
        //     stream.ToArray(),
        //     "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        //     "CompetenceDocument.docx"
        // );
    }
}