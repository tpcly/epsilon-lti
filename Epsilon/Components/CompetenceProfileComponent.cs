using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace Epsilon.Components;

public class CompetenceProfileComponent : AbstractCompetenceComponent
{
    public override async Task<Body?> AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        
        var body = mainDocumentPart.Document.Body;
        
        if (body == null)
        {
            return body;
        }
        
        body.AppendChild(FormattedText("Competence Profile"));  
        body.AppendChild(FormattedText(" "));
        
        var outcomes = 
            Submissions.ToEnumerable()
                       .SelectMany(static o => o.Results.
                                                 Select(static r => r.Outcome)).ToList();

        foreach (var domain in Domains)
        {
            if (domain!.ColumnsSet != null)
            {
                body.AppendChild(GetTableTwoAxis(domain, outcomes));
            }
            else
            {
                body.AppendChild(GetTableOneAxis(domain, outcomes));
            }
            body.AppendChild(
                FormattedText("")
            );   
        }
        
        return body;
    }

    private static OpenXmlElement GetTableOneAxis(LearningDomain domain, List<LearningDomainOutcome> outcomes)
    {
        var table = FormattedTable();

        var headerRow = new TableRow()
        {
            TableRowProperties = new TableRowProperties(
                new TableJustification(){Val = TableRowAlignmentValues.Center,}),
        };
        
        foreach (var row in domain.RowsSet.Types.OrderBy(static c => c.Order))
        {
            
            var cell = FormattedTableCell(
                "700", 
                new Paragraph(
                    new Run(
                        new Text(row.Name)
                        ){RunProperties = new RunProperties(){FontSize = new FontSize(){Val = "16",},},}
                    )
                );
            
            headerRow.AppendChild(cell);
        }
        
        table.AppendChild(headerRow);

        var domainRow = new TableRow()
        {
            TableRowProperties = new TableRowProperties(
                new TableJustification(){Val = TableRowAlignmentValues.Center,},
                new TableRowHeight() { Val = 2600, }
                ),
        };
        

        domainRow.Append();

        foreach (var row in domain.RowsSet.Types.OrderBy(static c => c.Order))
        {
            var cellOutcomes = outcomes.Where(o => o.Row.Id == row.Id).ToList();
            var types = cellOutcomes.Select(static o => o.Value).ToList();
            var value = types.MaxBy(static t => t.Order);
            var count = cellOutcomes.Count.ToString(CultureInfo.InvariantCulture);  
                
            var contentCell = new Paragraph(new Run(new Text(count)))
            {
                ParagraphProperties = new ParagraphProperties() { Justification = new Justification() { Val = JustificationValues.Center, }, },
            };
                
            var cell = FormattedTableCell(
                "1000",
                contentCell);
                

            if (value != null)
            {
                cell.TableCellProperties?.AppendChild(new Shading { Fill = "#" + value.HexColor, });
            }
                
            domainRow.AppendChild(cell);
        }
            
        table.AppendChild(domainRow);
        
        return table;
    }

    private static OpenXmlElement GetTableTwoAxis(LearningDomain domain, List<LearningDomainOutcome> outcomes)
    {
        var table = FormattedTable();

        var headerRow = new TableRow()
        {
            TableRowProperties = new TableRowProperties(
                new TableJustification(){Val = TableRowAlignmentValues.Center,}),
        };

        var content = new Paragraph(new Run(new Text("")))
        {
            ParagraphProperties = new ParagraphProperties() { Justification = new Justification() { Val = JustificationValues.Center, }, },
        };

        // Empty top-left cell.
        headerRow.AppendChild(
            FormattedTableCell("700", 
                content));
        
        foreach (var col in domain.ColumnsSet.Types.OrderBy(static c => c.Order))
        {
            
            var cell = FormattedTableCell(
                "700", 
                new Paragraph(
                    new Run(
                        new Text(col.Name)
                        ){RunProperties = new RunProperties(){FontSize = new FontSize(){Val = "16",},},}
                    )
                );
            
            headerRow.AppendChild(cell);
        }
        
        table.AppendChild(headerRow);

        foreach (var row in domain.RowsSet.Types.OrderBy(static r => r.Order))
        {
            var domainRow = new TableRow()
            {
                TableRowProperties = new TableRowProperties(
                    new TableJustification(){Val = TableRowAlignmentValues.Center,},
                    new TableRowHeight() { Val = 500, }),
            };
            
            domainRow.AppendChild(
                FormattedTableCell(
                    "1000", 
                    new Paragraph(
                        new Run(
                            new Text(row.Name)
                            ){RunProperties = new RunProperties(){FontSize = new FontSize(){Val = "16",},},}
                        )));

            foreach (var col in domain.ColumnsSet.Types.OrderBy(static c => c.Order))
            {
                var cellOutcomes = outcomes.Where(o => o.Row.Id == row.Id && o.Column?.Id == col.Id).ToList();
                var types = cellOutcomes.Select(static o => o.Value).ToList();
                var value = types.MaxBy(static t => t.Order);
                var count = cellOutcomes.Count.ToString(CultureInfo.InvariantCulture);  
                
                var contentCell = new Paragraph(new Run(new Text(count)))
                {
                    ParagraphProperties = new ParagraphProperties() { Justification = new Justification() { Val = JustificationValues.Center, }, },
                };
                
                var cell = FormattedTableCell(
                    "1000",
                    contentCell);
                

                if (value != null)
                {
                    cell.TableCellProperties?.AppendChild(new Shading { Fill = "#" + value.HexColor, });
                }
                
                domainRow.AppendChild(cell);
            }
            
            table.AppendChild(domainRow);
        }

        return table;
    }
    
    
    public CompetenceProfileComponent(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains, IEnumerable<LearningDomainOutcome> outcomes)
        : base(submissions, domains, outcomes)
    {
    }
}