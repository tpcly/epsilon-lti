using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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
    public override async Task<Body> AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var outcomes = 
            Submissions.ToEnumerable()
                       .SelectMany(static o => o.Results.
                                                 Select(static r => r.Outcome));
        
        var body = new Body();

        foreach (var domain in Domains)
        {
            if (domain!.ColumnsSet != null)
            {
                body.AppendChild(GetTableTwoAxis(domain, outcomes));
            }
        }
        
        mainDocumentPart.Document.AppendChild(body);
        return body;
    }
    
    public static  async Task<IEnumerable<T1>> SelectManyAsync<T, T1>(IEnumerable<T> enumeration, Func<T, Task<IEnumerable<T1>>> func)
    {
        return (await Task.WhenAll(enumeration.Select(func))).SelectMany(static s => s);
    }

    /*private OpenXmlElement GetTableOneAxis(LearningDomainTypeSet valueSet, LearningDomainTypeSet axisSet)
    {
        var table = new Table();
        
        
        
        return table;
    }*/

    private OpenXmlElement GetTableTwoAxis(LearningDomain domain, IEnumerable<LearningDomainOutcome> outcomes)
    {
        var table = new Table();
        
        // Set table properties for formatting.
        table.AppendChild(new TableProperties(
            new TableWidth { Width = "8", Type = TableWidthUnitValues.Auto, }));

        var headerRow = new TableRow();

        // Empty top-left cell.
        headerRow.AppendChild(
            CreateTableCell("1500", 
                _headerTableCellBorders, 
                new Paragraph(new Run(new Text("")))));
        
        foreach (var col in domain.ColumnsSet.Types.OrderBy(static c => c.Order))
        {
            var cell = CreateTableCell("1500", _borderedTableCellBorders, new Paragraph(new Run(new Text(col.Name))));
            
            headerRow.AppendChild(cell);
        }
        
        table.AppendChild(headerRow);

        foreach (var row in domain.RowsSet.Types.OrderBy(static r => r.Order))
        {
            var domainRow = new TableRow();
            domainRow.AppendChild(
                CreateTableCell(
                    "1500", 
                    _borderedTableCellBorders, 
                    new Paragraph(new Run(new Text(row.Name)))));

            foreach (var col in domain.ColumnsSet.Types.OrderBy(static c => c.Order))
            {
                var cellOutcomes = outcomes.Where(o => o.Row.Id == row.Id && o.Column.Id == col.Id);
                var count = cellOutcomes.Count().ToString(CultureInfo.InvariantCulture);                    
                
                var cell = CreateTableCell(
                    "1500",
                    _borderedTableCellBorders,
                    new Paragraph(new Run(new Text(count))));
                
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