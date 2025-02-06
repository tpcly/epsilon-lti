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
        

        // Create the table header row.
        var headerRow = new TableRow();
        
        var cellName = CreateTableCell("100");
        
        cellName.Append(CreateText("Outcome"));
        headerRow.AppendChild(cellName);
        
        var cellDescription = CreateTableCell("100");
        
        cellDescription.Append(CreateText("Description"));
        headerRow.AppendChild(cellDescription);

        table.AppendChild(headerRow);


        var listOfCriteria = Submissions.SelectMany(static e => e.Criteria.Select(static result => result));

        // Add the outcome rows.
        foreach (var outcomeCriterion in listOfCriteria.Distinct().OrderBy(static a => a.Id).Where(a => Outcomes.Where(o => o!.Id == a.Id).Any()))
        {
            
            var outcome = Outcomes.FirstOrDefault(o => o?.Id == outcomeCriterion.Id);
            
            if (outcome != null)
            {
                var row = new TableRow();
                var paragraphForOutcomeName = new Paragraph(new Run(new Text(outcome.Name)))
                {
                    ParagraphProperties = new ParagraphProperties { Justification = new Justification { Val = JustificationValues.Left, }, },
                };
                var paragraphForOutcomeDescription = new Paragraph(new Run(new Text(outcome.Description)))
                {
                    ParagraphProperties = new ParagraphProperties { Justification = new Justification { Val = JustificationValues.Left, }, },
                };
                row.AppendChild(CreateTableCell("1000", paragraphForOutcomeName));
                row.AppendChild(CreateTableCell("25000", paragraphForOutcomeDescription));
                table.AppendChild(row);
            }
        }
        
        body.AppendChild(table);
        return body;
    }
}