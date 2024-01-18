using System.Globalization;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;

namespace Epsilon.Components;

public class CompetenceProfileComponent : AbstractCompetenceComponent
{
    public override async Task<Body> AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var body = new Body();

        body.Append(new Paragraph(new Run(new Text("Hello World!"))));
        // toDo: Add the table with realised outcomes, like the performance dashboard.

        var count = await Submissions.CountAsync();
        body.Append(new Paragraph(new Run(new Text(count.ToString(CultureInfo.InvariantCulture)))));
        await foreach (var sub in Submissions)
        {
            body.Append(new Paragraph(new Run(new Text(sub.Assignment ?? "We tried"))));
        }

        mainDocumentPart.Document.AppendChild(body);
        return body;
    }

    public CompetenceProfileComponent(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains)
        : base(submissions, domains)
    {
    }
}