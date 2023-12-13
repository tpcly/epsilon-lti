using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Components;

public record PageComponent(string Html) : IWordCompetenceComponent
{
    public async Task<Body> AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var buffer = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes($"<html>{Html}</html>")).ToArray();
        using var stream = new MemoryStream(buffer);

        var formatImportPart = mainDocumentPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html);
        formatImportPart.FeedData(stream);
        var body = new Body(
            new AltChunk { Id = mainDocumentPart.GetIdOfPart(formatImportPart), }
        );

        mainDocumentPart.Document.AppendChild(body);
        return body;
    }
}