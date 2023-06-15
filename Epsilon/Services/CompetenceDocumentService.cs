using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using Epsilon.Abstractions.Services;
using Epsilon.Components;

namespace Epsilon.Services;

public class CompetenceDocumentService : ICompetenceDocumentService
{
    private readonly PageComponentFetcher _pageComponent;

    public CompetenceDocumentService(PageComponentFetcher pageComponent)
    {
        _pageComponent = pageComponent;
    }

    public async Task<CompetenceDocument> GetDocument(DateTime from, DateTime to)
    {
        var components = await FetchComponents().ToListAsync();

        return new CompetenceDocument(components);
    }

    public async Task<Stream> WriteDocument(Stream stream, CompetenceDocument document)
    {
        var startPosition = stream.Position;

        using var wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        wordDocument.AddMainDocumentPart();
        wordDocument.MainDocumentPart!.Document = new Document();

        foreach (var competenceWordComponent in document.Components)
        {
            competenceWordComponent.AddToWordDocument(wordDocument.MainDocumentPart);
        }

        wordDocument.Save();
        wordDocument.Close();

        // Reset stream position to start position
        stream.Position = startPosition;

        return stream;
    }

    private async IAsyncEnumerable<IWordCompetenceComponent> FetchComponents()
    {
        yield return await _pageComponent.Fetch("homepage");
        yield return await _pageComponent.Fetch("projects");
    }
}