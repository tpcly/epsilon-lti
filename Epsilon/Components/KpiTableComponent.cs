using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using System.Globalization;

namespace Epsilon.Components;

public class KpiTableComponent : AbstractCompetenceComponent
{
    public override async Task<Body> AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var body = mainDocumentPart.Document.Body;

        // Create a table to display outcomes, assignments, and grades
        var table = new Table();
        
        // Define table properties
        var tblProp = new TableProperties(
            new TableWidth(),
            new TableBorders(
                new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,  },
                new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,  },
                new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,  },
                new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,  }
            )
        );
        table.AppendChild<TableProperties>(tblProp);
        
        // Define table grid
        var tblGrid = new TableGrid();
        table.AppendChild(tblGrid);

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

                        var hyperlink = new Hyperlink(new Run(new Text(submission.Assignment!))) { Id = relationshipId, };
                        
                        assignmentsParagraph.AppendChild(hyperlink);
                        assignmentsParagraph.AppendChild(new Run(new Break()));

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
        
        return null;
    }

    private static TableCell CreateTableCellWithBorders(string? width, params OpenXmlElement[] elements)
    {
        var cell = new TableCell();
        var cellProperties = new TableCellProperties();

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

        cell.PrependChild(cellProperties);

        return cell;
    }

    public KpiTableComponent(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains)
        : base(submissions, domains)
    {
    }
}