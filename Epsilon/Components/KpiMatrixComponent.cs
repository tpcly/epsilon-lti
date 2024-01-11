using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;

namespace Epsilon.Components;

public class KpiMatrixComponent : AbstractCompetenceComponent
{

    public KpiMatrixComponent(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains, IEnumerable<LearningDomainOutcome> outcomes)
        : base(submissions, domains, outcomes)
    {
    }

    public override async Task<Body?> AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
         var body = mainDocumentPart.Document.Body;
        // Create a table, with rows for the outcomes and columns for the assignments.
        var table = new Table();


        // Set table properties for formatting.
        table.AppendChild(new TableProperties(
            new TableWidth { Width = "0", Type = TableWidthUnitValues.Auto, }));
        table.AppendChild(new TableGrid());
        // Calculate the header row height based on the longest assignment name.
        var headerRowHeight = 0;
        var anyAssignments = false;

        await foreach (var sub in Submissions)
        {
            anyAssignments = true;
            headerRowHeight = Math.Max(headerRowHeight, sub.Assignment?.Length ?? 10);

        }

        if (anyAssignments)
        {
            headerRowHeight *= 111;
        }

        // Create the table header row.
        var headerRow = new TableRow();
        headerRow.AppendChild(new TableRowProperties(new TableRowHeight { Val = (UInt32Value)(uint)headerRowHeight, }));
        
        // Empty top-left cell.
        headerRow.AppendChild(CompetenceProfileComponent.CreateTableCell("2500", GetBorders(), new Paragraph(new Run(new Text("")))));
        var index = 0;
        await foreach (var sub in Submissions)
        {
            var cell = CompetenceProfileComponent.CreateTableCell("100", GetBorders(), null);

            if (cell.FirstChild != null)
            {
                cell.FirstChild.Append(new TextDirection { Val = TextDirectionValues.TopToBottomLeftToRightRotated, });
                cell.FirstChild.Append(new Shading
                {
                    Val = ShadingPatternValues.Clear,
                    Fill = index % 2 == 0 ? "FFFFFF" : "d3d3d3",
                });
                cell.Append(new Paragraph(new Run(new Text(sub.Assignment ?? "Not found"))));
                headerRow.AppendChild(cell);
            }

            index++;
        }

        table.AppendChild(headerRow);


        var listOfCriteria = Submissions
            .SelectMany(static e => e.Criteria
                                     .Select(static result => result )
                                     .ToAsyncEnumerable());

        // Add the outcome rows.
        await foreach (var outcomeCriterion in listOfCriteria)
        {
            var row = new TableRow();
            var outcome = Outcomes.FirstOrDefault(o => o.Id == outcomeCriterion.Id);

            // Create a new paragraph for outcome.Name
            if (outcome != null)
            {
                var paragraphForOutcomeName = new Paragraph(new Run(new Text(outcome.Name)))
                {
                    ParagraphProperties = new ParagraphProperties { Justification = new Justification { Val = JustificationValues.Center, }, },
                };
                row.AppendChild(CompetenceProfileComponent.CreateTableCell("2500", GetBorders(), paragraphForOutcomeName));
            }

            await foreach (var sub in Submissions)
            {
                var cell = CompetenceProfileComponent.CreateTableCell("100", GetBorders(), null);
                var criteria = sub.Criteria.FirstOrDefault(c => c.Id == outcome?.Id);
                var result = sub.Results.FirstOrDefault(r => r.Outcome.Id == outcome?.Id);

                var status = GetStatus(result?.Grade, criteria?.MasteryPoints);
                var fillColor = GetColor(status);
                cell.FirstChild?.Append(new Shading { Val = ShadingPatternValues.Clear, Fill = fillColor, });

                var text = result != null ? result.Outcome.Value.ShortName : "";
                var paragraph = new Paragraph();
                var run = new Run(new Text(text));
                paragraph.Append(run);
                paragraph.ParagraphProperties = new ParagraphProperties()
                {
                    Justification = new Justification() { Val = JustificationValues.Center, },
                };

                cell.Append(paragraph);
                row.AppendChild(cell);
            }

            table.AppendChild(row);
        }
        
        body?.Append(new Paragraph(new Run(new Text(""))));
        body?.AppendChild(table);
        return body;
    }
    
    private static OutcomeGradeStatus GetStatus(double? grade, double? masteryPoints)
    {
        if (grade.HasValue && masteryPoints.HasValue)
        {
            return grade.Value >= masteryPoints.Value ? OutcomeGradeStatus.Mastered : OutcomeGradeStatus.NotMastered;
        }
        if (!grade.HasValue && masteryPoints.HasValue)
        {
            return OutcomeGradeStatus.NotAssessed;
        }
        return OutcomeGradeStatus.NotGraded;
    }

    private static string GetColor(OutcomeGradeStatus status)
    {
        return status switch
        {
            OutcomeGradeStatus.Mastered => "#44F656",
            OutcomeGradeStatus.NotMastered => "#FA1818",
            OutcomeGradeStatus.NotAssessed => "#9F2B68",
            OutcomeGradeStatus.NotGraded => "",
            _ => "",
        };
    }

    private static TableCellBorders GetBorders()
    {
        return  new TableCellBorders(
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
    }
}