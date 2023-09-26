using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;

namespace Epsilon.Components;

public record LearningDomainComponent(
    IEnumerable<LearningDomainOutcome> DomainOutcomes,
    LearningDomain Domain,
    IAsyncEnumerable<LearningDomainSubmission> Submissions
): IWordCompetenceComponent
{
    public void AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var body = new Body();
        
        body.Append(new Paragraph(new Run(new Text("Hello World!"))));
        // toDo: Add the table with realised outcomes, like the performance dashboard.
        
        mainDocumentPart.Document.AppendChild(body);
    }
}