using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using BottomBorder = DocumentFormat.OpenXml.Wordprocessing.BottomBorder;
using RightBorder = DocumentFormat.OpenXml.Wordprocessing.RightBorder;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
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
        
        body.AppendChild(

            new Paragraph(
                new Run(
                    new Text("Competence profile")
                )
                )
        );  
        body.AppendChild( new Paragraph(
            new Run(
                new Text(" ")
            )
        ));
        
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
                new Paragraph(
                    new Run(
                        new Text(" ")
                    )
                )
            );   
        }
        
        return body;
    }

    private static OpenXmlElement GetTableOneAxis(LearningDomain domain, List<LearningDomainOutcome> outcomes)
    {
        var table = new Table();
        
        var tblProp = new TableProperties(
            new TableWidth(),
            new TableBorders(
                new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,  },
                new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,  },
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

        var headerRow = new TableRow()
        {
            TableRowProperties = new TableRowProperties(
                new TableJustification(){Val = TableRowAlignmentValues.Center,}),
        };
        
        foreach (var row in domain.RowsSet.Types.OrderBy(static c => c.Order))
        {
            
            var cell = CreateTableCell(
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
                
            var cell = CreateTableCell(
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
        var table = new Table();
        
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
            CreateTableCell("700", 
                content));
        
        foreach (var col in domain.ColumnsSet.Types.OrderBy(static c => c.Order))
        {
            
            var cell = CreateTableCell(
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
                CreateTableCell(
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
                
                var cell = CreateTableCell(
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
    
    private static TableCell CreateTableCell(string? width, params OpenXmlElement[] elements)
    {
        width ??= "300";
        var cell = new TableCell();
        var cellProperties = new TableCellProperties()
        {
            TableCellVerticalAlignment = new TableCellVerticalAlignment() {Val = TableVerticalAlignmentValues.Center,},
            TableCellWidth = new TableCellWidth()
            {
                Type = TableWidthUnitValues.Dxa,
                Width = width,
            },
        };
        
        cell.TableCellProperties = cellProperties;
        
        foreach (var element in elements)
        {
            cell.Append(element);
        }

        return cell;
    }
    
    public CompetenceProfileComponent(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains)
        : base(submissions, domains)
    {
    }
}