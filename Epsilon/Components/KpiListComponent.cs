using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace Epsilon.Components;

public class KpiListComponent : AbstractCompetenceComponent
{
    public KpiListComponent(
        IEnumerable<LearningDomainSubmission> submissions,
        IEnumerable<LearningDomain?> domains,
        IEnumerable<LearningDomainOutcome?> outcomes
    )
        : base(submissions, domains, outcomes)
    {
    }

    public override async Task<Body?> AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var body = mainDocumentPart.Document.Body ?? throw new InvalidOperationException("The main document part does not contain a body.");
        
        body.AppendChild(CreateWhiteSpace());
        
        var table = CreateTable();

        // Calculate the header row height based on the longest assignment name.
        var headerRowHeight = Submissions.Max(static s => s.Assignment?.Length ?? 10);

        if (Submissions.Any())
            headerRowHeight *= 111;

        // Create the table header row.
        var headerRow = new TableRow();
        headerRow.AppendChild(new TableRowProperties(new TableRowHeight { Val = (UInt32Value)(uint)headerRowHeight, }));

        // Empty top-left cell.
        headerRow.AppendChild(CreateTableCell("2500", CreateWhiteSpace()));

        // var index = 0;
        // foreach (var sub in Submissions)
        // {
        //     var shading = new Shading
        //     {
        //         Val = ShadingPatternValues.Clear,
        //         Fill = index % 2 == 0
        //             ? "FFFFFF"
        //             : "d3d3d3",
        //     };
        //
        //     var cell = CreateTableCell("100", shading);
        //     cell.TableCellProperties!.TextDirection = new TextDirection { Val = TextDirectionValues.TopToBottomLeftToRightRotated, };
        //
        //     cell.Append(CreateText(sub.Assignment ?? "Not found"));
        //     headerRow.AppendChild(cell);
        //
        //     index++;
        // }

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
                var paragraphForOutcomeDescription = new Paragraph(new Run(new Text(outcome.Description)))
                {
                    ParagraphProperties = new ParagraphProperties { Justification = new Justification { Val = JustificationValues.Center, }, },
                };
                row.AppendChild(CreateTableCell("2500", paragraphForOutcomeName));
                row.AppendChild(CreateTableCell("2500", paragraphForOutcomeDescription));
            }

            table.AppendChild(row);
        }
        
        body.AppendChild(table);
        return body;
    }
}