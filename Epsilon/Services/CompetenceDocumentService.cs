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

    public CompetenceDocument GetDocument(string userId, DateTime? from = null, DateTime? to = null)
    {
        var submissions = _canvasResultService.GetSubmissions(userId);
        if (from != null && to != null)
        {
            submissions = submissions.Where(s => s.SubmittedAt <= from && s.SubmittedAt >= to);
        }

        var components = FetchComponents(submissions);

        return new CompetenceDocument(components);
    }

    public static async Task<bool> WriteDocument(Stream stream, CompetenceDocument document)
    {
        using var wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        wordDocument.AddMainDocumentPart();
        wordDocument.MainDocumentPart!.Document = new Document();

        await foreach (var competenceWordComponent in document.Components)
        {
            competenceWordComponent.AddToWordDocument(wordDocument.MainDocumentPart);
        }

        wordDocument.Save();
        wordDocument.Dispose();
        return true;
    }

    private async IAsyncEnumerable<AbstractCompetenceComponent> FetchComponents(IAsyncEnumerable<LearningDomainSubmission> submissions)
    {
        yield return new CompetenceProfileComponent(submissions, await _domainService.GetDomainsFromTenant());
        yield return new KpiTable(submissions, await _domainService.GetDomainsFromTenant());
    }
}