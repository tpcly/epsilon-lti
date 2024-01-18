using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Components;

public abstract class AbstractCompetenceComponent : IWordCompetenceComponent
{
    protected AbstractCompetenceComponent(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains)
    {
        Submissions = submissions;
        Domains = domains;
    }

    protected IAsyncEnumerable<LearningDomainSubmission> Submissions { get; set; }
    protected IEnumerable<LearningDomain?> Domains { get; set; }

    public abstract Task<Body> AddToWordDocument(MainDocumentPart mainDocumentPart);
}