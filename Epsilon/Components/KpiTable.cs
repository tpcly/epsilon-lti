using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using System.Globalization;

namespace Epsilon.Components;

public class KpiTable : AbstractCompetenceComponent
{
    public override async Task<Body>? AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var body = new Body();

        // Create a table to display outcomes, assignments, and grades
        var table = new Table();

        // Define column header texts
        var columnsHeaders = new Dictionary<string, string> { { "KPI", "3000" }, { "Assignments", "5000" }, { "Grades", "1000" }, }; 

        // Create the table header row
        var headerRow = new TableRow();

        // Create the header cells
        foreach (var columnHeader in columnsHeaders)
        {
            headerRow.AppendChild(CreateTableCellWithBorders(columnHeader.Value, new Paragraph(new Run(new Text(columnHeader.Key)))));
        }

        // Add the header row to the table
        table.AppendChild(headerRow);

        var allOutcomes = Submissions
            .SelectMany(static e => e.Results
                                     .Select(static result => result.Outcome)
                                     .ToAsyncEnumerable());

        // Create the table body rows and cells
        await foreach (var outcome in allOutcomes
                                      .OrderBy(static e => e.Value.Order)
                                      .Distinct())
        {
            var tableRow = new TableRow();
            
             // Outcome (KPI) column
            tableRow.AppendChild(CreateTableCellWithBorders("3000", new Paragraph(new Run(new Text(outcome.Name)))));
            
            // Assignments column
            var assignmentsParagraph = new Paragraph();
            var assignmentsRun = assignmentsParagraph.AppendChild(new Run());
            
            var gradesParagraph = new Paragraph();
            var gradesRun = gradesParagraph.AppendChild(new Run());

            await foreach (var submission in Submissions.Select(static e => e))
            {
                foreach (var result in submission.Results.Select(static e => e))
                {
                    if (result.Outcome.Id == outcome.Id)
                    {
                        var rel = mainDocumentPart.AddHyperlinkRelationship(new Uri(submission.AssignmentUrl!.ToString()), true);
                        var relationshipId = rel.Id;

                        var hyperlink = new Hyperlink(new RunProperties(
                            new Underline { Val = UnderlineValues.Single, }), new Text(submission.Assignment!)) { Id = relationshipId, };
                        
                        assignmentsRun.AppendChild(hyperlink);
                        assignmentsRun.AppendChild(new Paragraph());

                        var submissionGrade = result.Grade!.Value.ToString(CultureInfo.InvariantCulture);
                        
                        gradesRun.AppendChild(new Text(submissionGrade));
                        gradesRun.AppendChild(new Break());
                    }
                }
            }
            
            tableRow.AppendChild(CreateTableCellWithBorders("5000", assignmentsParagraph));
            tableRow.AppendChild(CreateTableCellWithBorders("1000", gradesParagraph));
            
             // Add the row to the table
            table.AppendChild(tableRow);
        }
        
        // Newline to separate the table from the rest of the document
        body.Append(new Paragraph(new Run(new Text(""))));
        
        // Add the table to the document
        body.AppendChild(table);

        mainDocumentPart.Document.AppendChild(body);
        return null;
    }

    private static TableCell CreateTableCellWithBorders(string? width, params OpenXmlElement[] elements)
    {
        var cell = new TableCell();
        var cellProperties = new TableCellProperties();
        var borders = new TableCellBorders(
            new LeftBorder
            {
                Val = BorderValues.Single,
            },
            new RightBorder
            {
                Val = BorderValues.Single,
            },
            new TopBorder
            {
                Val = BorderValues.Single,
            },
            new BottomBorder
            {
                Val = BorderValues.Single,
            });

        foreach (var element in elements)
        {
            cell.Append(element);
        }

        if (width != null)
        {
            cellProperties.Append(new TableCellWidth
            {
                Type = TableWidthUnitValues.Dxa,
                Width = width,
            });
        }

        cellProperties.Append(borders);
        cell.PrependChild(cellProperties);

        return cell;
    }

    public KpiTable(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains)
        : base(submissions, domains)
    {
    }
}