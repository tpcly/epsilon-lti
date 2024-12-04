using System.Collections.ObjectModel;
using System.Text;
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
    private readonly ISupplementDocumentService _supplementDocumentService;
    private readonly IEduBadgeService _eduBadgeService;

    public DocumentController(ICompetenceDocumentService competenceDocumentService, IEduBadgeService eduBadgeService, ISupplementDocumentService supplementDocumentService)
    {
        _competenceDocumentService = competenceDocumentService;
        _eduBadgeService = eduBadgeService;
        _supplementDocumentService = supplementDocumentService;
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
    
    [HttpGet("download/supplement")]
    public async Task<IActionResult> DownloadSupplement(string userId, string domains)
    {
        var document = _supplementDocumentService.GetDocument(userId, domains.Split(','));

        using var stream = new MemoryStream();
        await _supplementDocumentService.WriteDocument(stream, await document);

        return File(
            stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "SupplementDocument.docx"
        );
    }
    
    
    [HttpPost("download/edubadge/csv")]
    public async Task<IActionResult> DownloadCsv(Collection<string> searchQuery, DateTime from, DateTime to)
    {
        var data = await _eduBadgeService.GetData(searchQuery, from, to);
        var contents  = await _eduBadgeService.WriteDocument(data);
        
        var byteArray = Encoding.UTF8.GetBytes(contents);
        var stream = new MemoryStream(byteArray);
        
        return File(
            stream.ToArray(),
            "text/csv",
            "Edubadges.csv"
        );
    }
    
    
    
    // [HttpPost("download/diploma/json")]
    // public async Task<IActionResult> DownloadJson(Collection<string> searchQuery, DateTime from, DateTime to)
    // {
    //     var data = await _eduBadgeService.GetData(userIds, from, to);
    //     var contents  = await _eduBadgeService.WriteDocument(data);
    //     
    //     var byteArray = Encoding.UTF8.GetBytes(contents);
    //     var stream = new MemoryStream(byteArray);
    //     
    //     return File(
    //         stream.ToArray(),
    //         "text/csv",
    //         "Edubadges.csv"
    //     );
    // }
}