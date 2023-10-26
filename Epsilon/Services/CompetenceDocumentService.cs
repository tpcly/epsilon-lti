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
    private readonly ILearningDomainService _domainService;
    private readonly ILearningOutcomeCanvasResultService _canvasResultService;

    public CompetenceDocumentService(
        ILearningDomainService domainService,
        ILearningOutcomeCanvasResultService canvasResultService
    )
    {
        _domainService = domainService;
        _canvasResultService = canvasResultService;
    }

    public async Task<CompetenceDocument> GetDocument(string userId, DateTime? from = null, DateTime? to = null)
    {
        var submissions = _canvasResultService.GetSubmissions(userId);
        if (from != null && to != null)
        {
            submissions = submissions.Where(s => s.SubmittedAt <= from && s.SubmittedAt >= to);
        }

        var components = await FetchComponents(submissions).ToListAsync();

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

    private async IAsyncEnumerable<AbstractCompetenceComponent> FetchComponents(IAsyncEnumerable<LearningDomainSubmission> submissions)
    {
        yield return new CompetenceProfileComponent(submissions, await _domainService.GetDomainsFromTenant());
    }
}