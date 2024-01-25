using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Components;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace Epsilon.Components;

public class TitleComponent : IWordCompetenceComponent
{
    private readonly string _text;
    public async Task<Body?> AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        
        var body = mainDocumentPart.Document.Body;
        
        if (body == null)
        {
            return body;
        }

        body.AppendChild(new Paragraph(
            new Run(
                new Text(_text)
            )
        ));
        
        body.AppendChild(new Paragraph(
            new Run(
                new Text(" ")
            )
        ));

        return body;
    }


    public TitleComponent(string text)
    {
        _text = text;
    }
}