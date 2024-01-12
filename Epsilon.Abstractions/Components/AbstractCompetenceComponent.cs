using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Components;

public abstract class AbstractCompetenceComponent: IWordCompetenceComponent
{
    protected AbstractCompetenceComponent(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains, IEnumerable<LearningDomainOutcome> outcomes)
    {
        Submissions = submissions;
        Domains = domains;
        Outcomes = outcomes;
    }

    protected IAsyncEnumerable<LearningDomainSubmission> Submissions { get; set; }
    protected IEnumerable<LearningDomain?> Domains { get; set; }
    
    protected IEnumerable<LearningDomainOutcome> Outcomes { get; set; }

    public abstract Task<Body?> AddToWordDocument(MainDocumentPart mainDocumentPart);
}