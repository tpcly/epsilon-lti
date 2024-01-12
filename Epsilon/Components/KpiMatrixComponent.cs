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
        var table = new Table();

        table.AppendChild(new TableProperties(
            new TableWidth { Width = "0", Type = TableWidthUnitValues.Auto, }));
        table.AppendChild(new TableGrid());

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

        var index = 0;
        await foreach (var sub in Submissions)
        {
            var cell = CompetenceProfileComponent.CreateTableCell("100", _borderedTableCellBorders, null);
            
            if (cell.FirstChild != null)
            {
                cell.FirstChild.Append(new TextDirection { Val = TextDirectionValues.TopToBottomLeftToRightRotated, });

                cell.Append(new Paragraph(new Run(new Text(sub.Assignment ?? "Not found"))));
                headerRow.AppendChild(cell);
            }

            index++;
        }
        headerRow.AppendChild(new TableCell()); 
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
            
            if (outcome != null)
            {
                var paragraphForOutcomeName = new Paragraph(new Run(new Text(outcome.Name)))
                {
                    ParagraphProperties = new ParagraphProperties { Justification = new Justification { Val = JustificationValues.Center, }, },
                };
                row.AppendChild(CompetenceProfileComponent.CreateTableCell("2500", _borderedTableCellBorders, null, paragraphForOutcomeName));
            }
            var extraCell = new TableCell();
            extraCell.Append(new Paragraph(new Run(new Text("Extra Info")))); // Replace "Extra Info" with actual data
            row.AppendChild(extraCell);

            await foreach (var sub in Submissions)
            {
                var criteria = sub.Criteria.FirstOrDefault(c => c.Id == outcome?.Id);
                var result = sub.Results.FirstOrDefault(r => r.Outcome.Id == outcome?.Id);

                var status = GetStatus(result?.Grade, criteria?.MasteryPoints);
                var fillColor = GetColor(status);
                if (string.IsNullOrEmpty(fillColor))
                {
                    fillColor = "FFFFFF";
                }

                var shading = new Shading { Val = ShadingPatternValues.Clear, Fill = fillColor, };

                var cell = CompetenceProfileComponent.CreateTableCell("100", _borderedTableCellBorders, shading);

                var text = result != null
                    ? result.Outcome.Value.ShortName
                    : "";
                var paragraph = new Paragraph();
                var run = new Run(new Text(text));
                paragraph.Append(run);
                paragraph.ParagraphProperties = new ParagraphProperties() { Justification = new Justification() { Val = JustificationValues.Center, }, };

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
            OutcomeGradeStatus.NotGraded => "FFFFFF",
            _ => "FFFFFF",
        };
    }
    
    private readonly TableCellBorders _borderedTableCellBorders = new TableCellBorders(
        new TopBorder
        {
            Val = BorderValues.Single,
        },
        new LeftBorder
        {
            Val = BorderValues.Single,
        },
        new BottomBorder
        {
            Val = BorderValues.Single,
        },
        new RightBorder
        {
            Val = BorderValues.Single,
        });
}