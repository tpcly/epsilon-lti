using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using Epsilon.Abstractions.Services;
using Epsilon.Components;

namespace Epsilon.Services;

public class SupplementDocumentService : ISupplementDocumentService
{
    private readonly ILearningDomainService _domainService;
    private readonly ILearningOutcomeCanvasResultService _canvasResultService;

    public SupplementDocumentService(
        ILearningDomainService domainService,
        ILearningOutcomeCanvasResultService canvasResultService
    )
    {
        _domainService = domainService;
        _canvasResultService = canvasResultService;
    }

    public async Task<SupplementDocument> GetDocument(string userId, string[] domains)
    {
        var submissions = await _canvasResultService.GetSubmissions(userId)
                                              .Where(static e => e.Criteria.Any())
                                              .ToListAsync();

        var components = FetchComponents(submissions, domains);
        return new SupplementDocument(components);
    }

    public async Task<WordprocessingDocument> WriteDocument(Stream stream, SupplementDocument document)
    {
        var wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        wordDocument.AddMainDocumentPart();
        wordDocument.MainDocumentPart!.Document = new Document(new Body());

        await foreach (var competenceWordComponent in document.Components)
            await competenceWordComponent.AddToWordDocument(wordDocument.MainDocumentPart);

        wordDocument.Save();
        return wordDocument;
    }

    private async IAsyncEnumerable<IWordCompetenceComponent> FetchComponents(IList<LearningDomainSubmission> submissions, string[] usedDomains )
    {
        var domains = _domainService.GetDomainsFromTenant().ToList().FindAll(d => usedDomains.Contains(d?.Id));
        var outcomes = (await _domainService.GetOutcomes()).ToList().Where(static o => o!.Value.Order != 0);
        
        

        var learningDomainOutcomes = outcomes.ToList();
        yield return new TitleComponent("Applying Knowledge and Understanding");
        yield return new DiplomaSupplementCompetenceProfileComponent(submissions, domains, learningDomainOutcomes);
        yield return new TitleComponent("Performance indicators");
        yield return new KpiListComponent(submissions, domains, learningDomainOutcomes);
    }
}