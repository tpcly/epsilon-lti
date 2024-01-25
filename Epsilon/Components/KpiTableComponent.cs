using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using System.Globalization;

namespace Epsilon.Components;

public class KpiTableComponent : AbstractCompetenceComponent
{
    public KpiTableComponent(
        IEnumerable<LearningDomainSubmission> submissions,
        IEnumerable<LearningDomain?> domains,
        IEnumerable<LearningDomainOutcome?> outcomes
    )
        : base(submissions, domains, outcomes)
    {
    }

    public override async Task<Body?> AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var body = mainDocumentPart.Document.Body;

        if (body == null)
        {
            return body;
        }

        var table = CreateTable();
        
        var columnsHeaders = new Dictionary<string, string> { { "KPI", "3000" }, { "Assignments", "5000" }, { "Grades", "1000" }, };

        // Create the table header row
        var headerRow = new TableRow();
        
        foreach (var columnHeader in columnsHeaders)
        {
            headerRow.AppendChild(CreateTableCell(columnHeader.Value, CreateText(columnHeader.Key)));
        }
        
        table.AppendChild(headerRow);

        // Get allOutcomes
        var allOutcomes = Submissions
            .SelectMany(static e => e.Results
                                     .Select(static result => result.Outcome));

        foreach (var outcome in allOutcomes
                                .OrderBy(static e => e.Value.Order)
                                .Distinct())
        {
            var tableRow = new TableRow();

            // Outcome (KPI) column
            tableRow.AppendChild(CreateTableCell("3000", CreateText(outcome.Name)));

            // Assignments column
            var assignmentsParagraph = new Paragraph();

            var gradesParagraph = new Paragraph();
            var gradesRun = gradesParagraph.AppendChild(new Run());

            foreach (var submission in Submissions.Select(static e => e))
            {
                foreach (var result in submission.Results.Select(static e => e))
                {
                    if (result.Outcome.Id == outcome.Id)
                    {
                        var rel = mainDocumentPart.AddHyperlinkRelationship(new Uri(submission.AssignmentUrl!.ToString()), true);
                        var relationshipId = rel.Id;

                        var hyperlink
                            = new Hyperlink(new Run(new RunProperties(new Underline { Val = UnderlineValues.Single, }), new Text(submission.Assignment!))) { Id = relationshipId, };

                        assignmentsParagraph.AppendChild(hyperlink);
                        assignmentsParagraph.AppendChild(new Run(new Break()));

                        var submissionGrade = result.Grade!.Value.ToString(CultureInfo.InvariantCulture);

                        gradesRun.AppendChild(new Text(submissionGrade));
                        gradesRun.AppendChild(new Break());
                    }
                }
            }

            tableRow.AppendChild(CreateTableCell("5000", assignmentsParagraph));
            tableRow.AppendChild(CreateTableCell("1000", gradesParagraph));

            table.AppendChild(tableRow);
        }

        body.Append(CreateWhiteSpace());
        body.AppendChild(table);

        return body;
    }
}