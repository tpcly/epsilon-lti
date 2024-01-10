using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using BottomBorder = DocumentFormat.OpenXml.Wordprocessing.BottomBorder;
using LeftBorder = DocumentFormat.OpenXml.Wordprocessing.LeftBorder;
using RightBorder = DocumentFormat.OpenXml.Wordprocessing.RightBorder;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using TopBorder = DocumentFormat.OpenXml.Wordprocessing.TopBorder;

namespace Epsilon.Components;

public class CompetenceProfileComponent : AbstractCompetenceComponent
{
    public override async Task<Body?> AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var outcomes = 
            Submissions.ToEnumerable()
                       .SelectMany(static o => o.Results.
                                                 Select(static r => r.Outcome)).ToList();
        
        var body = mainDocumentPart.Document.Body;
        
        var table = new Table();
        
        // Define table properties
        var tblProp = new TableProperties(
            new TableWidth(),
            new TableBorders(
                new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.None), Size = 3,  },
                new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.None), Size = 3,  },
                new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.None), Size = 3,  },
                new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.None), Size = 3,  }
            )
        );
        table.AppendChild<TableProperties>(tblProp);
        
        // Define table grid
        var tblGrid = new TableGrid();
        table.AppendChild(tblGrid);
        
        var row = new TableRow();
        

        foreach (var domain in Domains)
        {
            if (domain!.ColumnsSet != null)
            {
                row.AppendChild(CreateTableCell("3000", _headerTableCellBorders, GetTableTwoAxis(domain, outcomes)));
            }
            else
            {
                row.AppendChild(CreateTableCell("3000", _headerTableCellBorders, GetTableOneAxis(domain, outcomes)));
            }

            
        }

        table.AppendChild(row);

        body?.AppendChild(table);
        
        mainDocumentPart.Document.AppendChild(body);
        return body;
    }

    private OpenXmlElement GetTableOneAxis(LearningDomain domain, List<LearningDomainOutcome> outcomes)
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

        var headerRow = new TableRow();
        
        foreach (var row in domain.RowsSet.Types.OrderBy(static c => c.Order))
        {
            
            var cell = CreateTableCell(
                "700", 
                _borderedTableCellBorders, 
                new Paragraph(
                    new Run(
                        new Text(row.Name)
                        ){RunProperties = new RunProperties(){FontSize = new FontSize(){Val = "14",},},}
                    )
                );
            
            headerRow.AppendChild(cell);
        }
        
        table.AppendChild(headerRow);
        
        var domainRow = new TableRow();
            

        domainRow.Append(new TableRowHeight(){Val = 2600,});

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
                _borderedTableCellBorders,
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

    private OpenXmlElement GetTableTwoAxis(LearningDomain domain, List<LearningDomainOutcome> outcomes)
    {
        var table = new Table();
        
        var tblProp = new TableProperties(
            new TableWidth(),
            new TableBorders(
                new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,  },
                new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 10,  },
                new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,  },
                new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 3,  }
            )
        );
        table.AppendChild<TableProperties>(tblProp);
        
        // Define table grid
        var tblGrid = new TableGrid();
        table.AppendChild(tblGrid);

        var headerRow = new TableRow();

        var content = new Paragraph(new Run(new Text("")))
        {
            ParagraphProperties = new ParagraphProperties() { Justification = new Justification() { Val = JustificationValues.Center, }, },
        };

        // Empty top-left cell.
        headerRow.AppendChild(
            CreateTableCell("700", 
                _headerTableCellBorders, content));
        
        foreach (var col in domain.ColumnsSet.Types.OrderBy(static c => c.Order))
        {
            
            var cell = CreateTableCell(
                "700", 
                _borderedTableCellBorders, 
                new Paragraph(
                    new Run(
                        new Text(col.Name)
                        ){RunProperties = new RunProperties(){FontSize = new FontSize(){Val = "14",},},}
                    )
                );
            
            headerRow.AppendChild(cell);
        }
        
        table.AppendChild(headerRow);

        foreach (var row in domain.RowsSet.Types.OrderBy(static r => r.Order))
        {
            var domainRow = new TableRow();
            

            domainRow.Append(new TableRowHeight(){Val = 500,});
            
            domainRow.AppendChild(
                CreateTableCell(
                    "1000", 
                    _borderedTableCellBorders, 
                    new Paragraph(
                        new Run(
                            new Text(row.Name)
                            ){RunProperties = new RunProperties(){FontSize = new FontSize(){Val = "14",},},}
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
                    _borderedTableCellBorders,
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
    
    private readonly TableCellBorders _borderedTableCellBorders = new TableCellBorders(
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
    
    private readonly TableCellBorders _headerTableCellBorders = new TableCellBorders(
        new LeftBorder
        {
            Val = BorderValues.None,
        },
        new RightBorder
        {
            Val = BorderValues.None,
        },
        new TopBorder
        {
            Val = BorderValues.None,
        },
        new BottomBorder
        {
            Val = BorderValues.None,
        });
    
    private static TableCell CreateTableCell(string? width, TableCellBorders borders, params OpenXmlElement[] elements)
    {
        width ??= "300";
        
        var cell = new TableCell();
        var cellProperties = new TableCellProperties()
        {
            TableCellBorders = (TableCellBorders)borders.CloneNode(true),
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