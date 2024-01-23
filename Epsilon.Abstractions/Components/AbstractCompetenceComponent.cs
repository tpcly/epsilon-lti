using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Components;

public abstract class AbstractCompetenceComponent: IWordCompetenceComponent
{
    protected AbstractCompetenceComponent(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains, IEnumerable<LearningDomainOutcome> outcomes)
    {
        Submissions = submissions;
        Domains = domains;
        Outcomes = outcomes;
    }

    protected IAsyncEnumerable<LearningDomainSubmission> Submissions { get; set; }
    protected IEnumerable<LearningDomain?> Domains { get; set; }
    
    protected IEnumerable<LearningDomainOutcome> Outcomes { get; set; }

    public abstract Task<Body?> AddToWordDocument(MainDocumentPart mainDocumentPart);

    protected static Paragraph CreateText(string text)
    {
        return new Paragraph(
            new Run(
                new Text(text)
            )
        );
    }
    
    protected static Paragraph CreateCenteredText(string text)
    {
        return new Paragraph(
            new Run(
                new Text(text)
            )
        ) { 
            ParagraphProperties = new ParagraphProperties() 
            {
                Justification = new Justification() { Val = JustificationValues.Center, },
            }, 
        };
    }
    
    protected static Paragraph CreateWhiteSpace()
    {
        return new Paragraph(
            new Run(
                new Text(" ")
            )
        );
    }

    protected static Table CreateTable()
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
        return table;
    }
    
    protected static TableCell CreateTableCell(string width, params OpenXmlElement[] elements)
    {
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
    
    protected static TableCell CreateTableCell(string width, Shading shading, params OpenXmlElement[] elements)
    {
        var cell = new TableCell();
        var cellProperties = new TableCellProperties()
        {
            TableCellVerticalAlignment = new TableCellVerticalAlignment() {Val = TableVerticalAlignmentValues.Center,},
            TableCellWidth = new TableCellWidth()
            {
                Type = TableWidthUnitValues.Dxa,
                Width = width,
            },
            Shading = (Shading)shading.CloneNode(true),
        };
        
        cell.TableCellProperties = cellProperties;
        
        foreach (var element in elements)
        {
            cell.Append(element);
        }

        return cell;
    }
    
    protected static TableCell CreateTableCell(string width, Shading shading, string text)
    {
        var cell = new TableCell();
        var cellProperties = new TableCellProperties()
        {
            TableCellVerticalAlignment = new TableCellVerticalAlignment() {Val = TableVerticalAlignmentValues.Center,},
            TableCellWidth = new TableCellWidth()
            {
                Type = TableWidthUnitValues.Dxa,
                Width = width,
            },
            Shading = (Shading)shading.CloneNode(true),
        };
        
        cell.TableCellProperties = cellProperties;
        
        cell.Append(CreateText(text));

        return cell;
    }
    
    protected static TableCell CreateTableCell(string width, string text)
    {
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
        
        cell.Append(CreateText(text));

        return cell;
    }
}