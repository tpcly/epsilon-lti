using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace Epsilon.Components;

public class KpiMatrixComponent : AbstractCompetenceComponent
{
    public KpiMatrixComponent(
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

        body.AppendChild(CreateLegend());
        body.AppendChild(CreateWhiteSpace());
        
        var table = CreateTable();

        // Calculate the header row height based on the longest assignment name.
        var headerRowHeight = Submissions.Max(static s => s.Assignment?.Length ?? 10);

        if (Submissions.Any())
        {
            headerRowHeight *= 111;
        }

        // Create the table header row.
        var headerRow = new TableRow();
        headerRow.AppendChild(new TableRowProperties(new TableRowHeight { Val = (UInt32Value)(uint)headerRowHeight, }));

        // Empty top-left cell.
        headerRow.AppendChild(CreateTableCell("2500", CreateWhiteSpace()));

        var index = 0;
        foreach (var sub in Submissions)
        {
            var shading = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Fill = index % 2 == 0
                    ? "FFFFFF"
                    : "d3d3d3",
            };

            var cell = CreateTableCell("100", shading);
            cell.TableCellProperties!.TextDirection = new TextDirection { Val = TextDirectionValues.TopToBottomLeftToRightRotated, };

            cell.Append(CreateText(sub.Assignment ?? "Not found"));
            headerRow.AppendChild(cell);

            index++;
        }

        table.AppendChild(headerRow);


        var listOfCriteria = Submissions.SelectMany(static e => e.Criteria.Select(static result => result));

        // Add the outcome rows.
        foreach (var outcomeCriterion in listOfCriteria.Distinct().OrderBy(static a => a.Id))
        {
            var row = new TableRow();
            var outcome = Outcomes.FirstOrDefault(o => o?.Id == outcomeCriterion.Id);

            // Create a new paragraph for outcome.Name
            if (outcome != null)
            {
                var paragraphForOutcomeName = new Paragraph(new Run(new Text(outcome.Name)))
                {
                    ParagraphProperties = new ParagraphProperties { Justification = new Justification { Val = JustificationValues.Center, }, },
                };
                row.AppendChild(CreateTableCell("2500", paragraphForOutcomeName));
            }

            foreach (var sub in Submissions)
            {
                var criteria = sub.Criteria.FirstOrDefault(c => c.Id == outcome?.Id);
                var result = sub.Results.FirstOrDefault(r => r.Outcome.Id == outcome?.Id);

                var status = GetStatus(result?.Grade, criteria?.MasteryPoints);
                var fillColor = GetColor(status);
                if (string.IsNullOrEmpty(fillColor))
                {
                    fillColor = "FFFFFF"; // default color code
                }

                var shading = new Shading { Val = ShadingPatternValues.Clear, Fill = fillColor, };
                var cell = CreateTableCell("100",
                    shading,
                    CreateCenteredText(result != null
                        ? result.Outcome.Value.ShortName
                        : ""));
                row.AppendChild(cell);
            }

            table.AppendChild(row);
        }
        
        body.AppendChild(table);
        return body;
    }

    private static OpenXmlElement CreateLegend()
    {
        var legend = CreateTable();
        foreach (OutcomeGradeStatus gradeStatus in Enum.GetValues(typeof(OutcomeGradeStatus)))
        {
            var legendRow = new TableRow();
            legendRow.AppendChild(CreateTableCell("300", new Shading(){Val = ShadingPatternValues.Clear,Fill = GetColor(gradeStatus), },CreateText("")));
            legendRow.AppendChild(CreateTableCell("300", CreateText(gradeStatus.ToString())));
            legend.AppendChild(legendRow);
        }
        var numberRow = new TableRow();
        numberRow.AppendChild(CreateTableCell("300", CreateText("1/2/3/4/5")));
        numberRow.AppendChild(CreateTableCell("300", CreateText("Grade")));
        legend.AppendChild(numberRow);
        
        return legend;
    }

    private static OutcomeGradeStatus? GetStatus(double? grade, double? masteryPoints)
    {
        if (grade.HasValue && masteryPoints.HasValue)
        {
            return grade.Value >= masteryPoints.Value
                ? OutcomeGradeStatus.Mastered
                : OutcomeGradeStatus.NotMastered;
        }

        if (!grade.HasValue && masteryPoints.HasValue)
        {
            return OutcomeGradeStatus.NotAssessed;
        }

        return null;
    }

    private static string GetColor(OutcomeGradeStatus? status)
    {
        return status switch
        {
            OutcomeGradeStatus.Mastered => "44F656",
            OutcomeGradeStatus.NotMastered => "FA1818",
            OutcomeGradeStatus.NotAssessed => "9F2B68",
            null => "FFFFFF",
            _ => "FFFFFF",
        };
    }
}