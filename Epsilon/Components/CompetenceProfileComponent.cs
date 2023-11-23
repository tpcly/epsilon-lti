using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;

namespace Epsilon.Components;

public class CompetenceProfileComponent: AbstractCompetenceComponent
{
    public override void AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var body = new Body();
        
        body.Append(new Paragraph(new Run(new Text("Hello World!"))));
        // toDo: Add the table with realised outcomes, like the performance dashboard.
        
        mainDocumentPart.Document.AppendChild(body);
    }

    public CompetenceProfileComponent(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains)
        : base(submissions, domains)
    {
    }
}