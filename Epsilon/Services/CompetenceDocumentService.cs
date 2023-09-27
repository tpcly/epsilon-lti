using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using Epsilon.Abstractions.Services;
using Epsilon.Components;

namespace Epsilon.Services;

public class CompetenceDocumentService : ICompetenceDocumentService
{
    private readonly IPageComponentManager _pageComponent;
    private readonly ILearningDomainService _domainService;
    private readonly ILearningOutcomeCanvasResultService _canvasResultService;

    public CompetenceDocumentService(
        IPageComponentManager pageComponent, 
        ILearningDomainService domainService, 
        ILearningOutcomeCanvasResultService canvasResultService)
    {
        _pageComponent = pageComponent;
        _domainService = domainService;
        _canvasResultService = canvasResultService;
    }

    public async Task<CompetenceDocument> GetDocument(int courseId, string userId, DateTime from, DateTime to)
    {
        var components = await FetchComponents(courseId, userId).ToListAsync();

        return new CompetenceDocument(components);
    }

    public void WriteDocument(Stream stream, CompetenceDocument document)
    {
        var startPosition = stream.Position;

        using var wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        wordDocument.AddMainDocumentPart();
        wordDocument.MainDocumentPart!.Document = new Document();

        foreach (var competenceWordComponent in document.Components)
        {
            competenceWordComponent.AddToWordDocument(wordDocument.MainDocumentPart);
        }

        wordDocument.Save();
        wordDocument.Close();

        // Reset stream position to start position
        stream.Position = startPosition;
    }

    private async IAsyncEnumerable<IWordCompetenceComponent> FetchComponents(int courseId, string userId)
    {
        var domain = await _domainService.GetDomain("hboi-1028");
        var domainOutcome = await _domainService.GetOutcomes();
        var submissions = _canvasResultService.GetSubmissions(userId);
        yield return await _pageComponent.Fetch(courseId, "homepage");
        yield return await _pageComponent.Fetch(courseId, "projects");
        yield return new CompetenceProfileComponent(domain, domainOutcome, submissions);
    }
}