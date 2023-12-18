using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;

namespace Epsilon.Components;

public class KpiMatrixComponent : AbstractCompetenceComponent
{

    public KpiMatrixComponent(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains)
        : base(submissions, domains)
    {
        
    }

    public override async Task<Body> AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var body = new Body();
        // Create a table, with rows for the outcomes and columns for the assignments.
        var table = new Table();
        
        var assignments = await Submissions.Select(static e => e.Assignment).ToListAsync();
        
        // Set table properties for formatting.
        table.AppendChild(new TableProperties(
            new TableWidth
            {
                Width = "0", Type = TableWidthUnitValues.Auto,
            }));

        // Calculate the header row height based on the longest assignment name.
        var headerRowHeight = 0;
        var anyAssignments = false;

        foreach (var assignment in assignments.Where(static a => a != null))
        {
            anyAssignments = true;
            if (assignment != null)
            {
                headerRowHeight = Math.Max(headerRowHeight, assignment.Length);
            }
        }

        if (anyAssignments)
        {
            headerRowHeight *= 111;
        }
        
        // Create the table header row.
        var headerRow = new TableRow();
        headerRow.AppendChild(new TableRowProperties(new TableRowHeight
        {
            Val = (UInt32Value)(uint)headerRowHeight,
        }));

        // Empty top-left cell.
        headerRow.AppendChild(CreateTableCellWithBorders("2500", new Paragraph(new Run(new Text("")))));

        foreach (var assignment in assignments)
        {
            var cell = CreateTableCellWithBorders("100");
            
            if (assignment != null)
            {
                if (cell.FirstChild != null)
                {
                    cell.FirstChild.Append(new TextDirection { Val = TextDirectionValues.TopToBottomLeftToRightRotated, });

                    cell.Append(new Paragraph(new Run(new Text(assignment))));
                    cell.FirstChild.Append(new Shading
                    {
                        Fill = assignments.IndexOf(assignment) % 2 == 0
                            ? "FFFFFF"
                            : "d3d3d3",
                    });
                    headerRow.AppendChild(cell);
                }
            }
        }

        table.AppendChild(headerRow);


        var listOfOutcomes = Submissions
            .SelectMany(static e => e.Results
                                     .Select(static result => new { result.Outcome.Id, result.Outcome,  })
                                     .ToAsyncEnumerable());

        // Add the outcome rows.
        await foreach (var outcome in listOfOutcomes.OrderByDescending(static o => o.Outcome.Name))
        {
            var row = new TableRow();

            // Add the outcome title cell.
            row.AppendChild(CreateTableCellWithBorders("2500", new Paragraph(new Run(new Text(outcome.Outcome.Name)))));

            // Add the assignment cells.
             foreach (var assignment in assignments)
            {
                var outcomeAssignment = assignment.Outcomes.FirstOrDefault(o => o.Id == outcome.Key);
                var cell = CreateTableCellWithBorders("100");

                // Set cell color based on GradeStatus.
                var fillColor = outcomeAssignment != null
                    ? outcomeAssignment.GradeStatus.Color
                    : assignments.IndexOf(assignment) % 2 == 0
                        ? "ffffff"
                        : "d3d3d3";

                cell.FirstChild?.Append(new Shading
                {
                    Fill = fillColor,
                });


                cell.FirstChild?.Append(new Shading
                {
                    Fill = fillColor,
                });

                // Add an empty text element since we're using color instead of text.
                cell.Append(new Paragraph(new Run(new Text(""))));
                row.AppendChild(cell);
            }

            table.AppendChild(row);
        }


        body.AppendChild(GetLegend());
        body.Append(new Paragraph(new Run(new Text(""))));
        body.AppendChild(table);

        mainDocumentPart.Document.AppendChild(body);
    }
    
    private OpenXmlElement GetLegend()
    {
        var table = new Table();
        foreach (var status in GradeStatus)
        {
            var row = new TableRow();
            var cellName = CreateTableCellWithBorders("200");
            cellName.Append(new Paragraph(new Run(new Text(status.Value.Status))));

            var cellValue = CreateTableCellWithBorders("200");
            cellValue.Append(new Paragraph(new Run(new Text(""))));
            cellValue.FirstChild?.Append(new Shading
            {
                Fill = status.Value.Color,
            });
            row.AppendChild(cellName);
            row.AppendChild(cellValue);
            table.AppendChild(row);
        }

        return table;
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
                Type = TableWidthUnitValues.Dxa, Width = width,
            });
        }

        cellProperties.Append(borders);
        cell.PrependChild(cellProperties);

        return cell;
    }
}