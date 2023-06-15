using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using Epsilon.Abstractions.Services;

namespace Epsilon.Services;

public class CompetenceDocumentService : ICompetenceDocumentService
{
    private readonly IEnumerable<IFetcher<PageComponent>> _pageComponents;

    public CompetenceDocumentService(IEnumerable<IFetcher<PageComponent>> pageComponents)
    {
        _pageComponents = pageComponents;
    }
    
    public async Task<CompetenceDocument> GetDocument(DateTime from, DateTime to)
    {
        var components = await FetchComponents(from, to).ToListAsync();

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

    private async IAsyncEnumerable<IWordCompetenceComponent> FetchComponents(DateTime from, DateTime to)
    {
        foreach (var pageComponentFetcher in _pageComponents)
        {
            yield return await pageComponentFetcher.Fetch(from, to);
        }
    }
}