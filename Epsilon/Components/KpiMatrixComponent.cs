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
            new TableWidth { Width = "0", Type = TableWidthUnitValues.Auto, }));

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
        headerRow.AppendChild(new TableRowProperties(new TableRowHeight { Val = (UInt32Value)(uint)headerRowHeight, }));

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
                                     .Select(static result => new { result.Outcome.Id, result.Outcome, })
                                     .ToAsyncEnumerable());

        // Add the outcome rows.
        await foreach (var outcome in listOfOutcomes.OrderByDescending(static o => o.Outcome.Value))
        {
            var row = new TableRow();

            // Add the outcome title cell.
            row.AppendChild(CreateTableCellWithBorders("2500", new Paragraph(new Run(new Text(outcome.Outcome.Value.Name)))));

            // Add the assignment cells.
            foreach (var assignment in assignments)
            {
                if (assignment == outcome.Outcome.Name)
                {
                    var outcomeAssignment = outcome.Outcome;
                    
                    var cell = CreateTableCellWithBorders("100");

                    // Set cell color based on GradeStatus.
                    var fillColor = outcomeAssignment.Value.HexColor;

                    cell.FirstChild?.Append(new Shading { Fill = fillColor, });

                    // Add an empty text element since we're using color instead of text.
                    cell.Append(new Paragraph(new Run(new Text(""))));
                    row.AppendChild(cell);
                }
            }

            table.AppendChild(row);
        }

        body.AppendChild(await GetLegend());
        body.Append(new Paragraph(new Run(new Text(""))));
        body.AppendChild(table);

        return mainDocumentPart.Document.AppendChild(body);
    }

    private static string GetColor(OutcomeGradeStatus status)
    {
        return status switch
        {
            OutcomeGradeStatus.Mastered => "#44F656",
            OutcomeGradeStatus.NotMastered => "#FA1818",
            OutcomeGradeStatus.NotGraded => "#FAFF00",
            OutcomeGradeStatus.NotAssessed => "#9F2B68",
            _ => "#9F2B68",
        };
    }
    
    private static OutcomeGradeStatus GetStatus(double? grade, double? masteryPoints)
    {
        if (grade.HasValue && masteryPoints.HasValue)
        {
            return grade.Value >= masteryPoints.Value ? OutcomeGradeStatus.Mastered : OutcomeGradeStatus.NotMastered;
        }
        else if (masteryPoints.HasValue)
        {
            return OutcomeGradeStatus.NotAssessed;
        }
        return OutcomeGradeStatus.NotGraded;
    }
    
    private async Task<HashSet<LearningDomainOutcome>> GetAllOutcomesAsync()
    {
        var outcomes = new HashSet<LearningDomainOutcome>();
        var submissions = await Submissions.ToListAsync();

        foreach (var submission in submissions)
        {
            foreach (var result in submission.Results)
            {
                outcomes.Add(result.Outcome);
            }
        }
        return outcomes;
    }

    private async Task<OpenXmlElement> GetLegend()
    {
        var table = new Table();
        var submissions = await Submissions.ToListAsync();
        var allOutcomes = await GetAllOutcomesAsync();
        foreach (var outcome in allOutcomes)
        {
            foreach (var submission in submissions)
            {
                var criteria = submission.Criteria.FirstOrDefault(c => c.Id == outcome.Id);
                var result = submission.Results.FirstOrDefault(r => r.Outcome.Id == outcome.Id);
                var status = GetStatus(result?.Grade, criteria?.MasteryPoints);
                var color = GetColor(status);
                
                var row = new TableRow();
                var cellName = CreateTableCellWithBorders("200");
                cellName.Append(new Paragraph(new Run(new Text(status.ToString()))));

                var cellValue = CreateTableCellWithBorders("200");
                cellValue.Append(new Paragraph(new Run(new Text(""))));
                cellValue.FirstChild?.Append(new Shading { Fill = color, });
                row.AppendChild(cellName);
                row.AppendChild(cellValue);
                table.AppendChild(row);
            }
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