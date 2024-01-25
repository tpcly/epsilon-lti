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
        submissions = submissions.Where(static e => e.Criteria.Any());

        if (from != null && to != null)
        {
            var components = FetchComponents(submissions, from.Value, to.Value);
            return new CompetenceDocument(components);
        }

        throw new ArgumentNullException(nameof(from));

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

    public async IAsyncEnumerable<IWordCompetenceComponent> FetchComponents(IAsyncEnumerable<LearningDomainSubmission> submissions, DateTime from, DateTime to )
    {
        var domains = await _domainService.GetDomainsFromTenant();
        var outcomes = await _domainService.GetOutcomes();
        var delta = submissions.Where(s => s.SubmittedAt >= from && s.SubmittedAt <= to);
        var startSubmissions = submissions.Where(s => s.SubmittedAt <= from);

        yield return new TitleComponent("Starting profile");
        yield return new CompetenceProfileComponent(startSubmissions, domains, outcomes);
        yield return new TitleComponent("Intended development");
        yield return new CompetenceProfileComponent(submissions.Where(static s=> s.SubmittedAt == null), domains, outcomes);
        yield return new TitleComponent("Final development");
        yield return new CompetenceProfileComponent(submissions, domains, outcomes);
        yield return new KpiTableComponent(delta, domains, outcomes);
        yield return new KpiMatrixComponent(delta, domains, outcomes);
    }
}