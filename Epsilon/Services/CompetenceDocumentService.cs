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
            submissions = submissions.Where(s => s.SubmittedAt >= from && s.SubmittedAt <= to);
        }

        var domains = await _domainService.GetDomainsFromTenant();
        var components = FetchComponents(submissions, domains);

        return new CompetenceDocument(components);
    }

    public async Task<WordprocessingDocument> WriteDocument(Stream stream, CompetenceDocument document)
    {
        var wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        wordDocument.AddMainDocumentPart();
        wordDocument.MainDocumentPart!.Document = new Document(new Body());

        foreach (var competenceWordComponent in await document.Components.ToListAsync())
        {
            await competenceWordComponent.AddToWordDocument(wordDocument.MainDocumentPart);
        }

        wordDocument.Save();
        return wordDocument;
    }

    public static async IAsyncEnumerable<AbstractCompetenceComponent> FetchComponents(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains)
    {
        yield return new CompetenceProfileComponent(submissions, domains);
    }
}