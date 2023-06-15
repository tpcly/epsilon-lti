using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
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

    public async Task<Stream> WriteDocument(Stream stream, DateTime from, DateTime to)
    {
        var components = await FetchComponents(from, to).ToListAsync();
        var startPosition = stream.Position;

        using var document = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        document.AddMainDocumentPart();
        document.MainDocumentPart!.Document = new Document();

        foreach (var competenceWordComponent in components)
        {
            competenceWordComponent.AddToWordDocument(document.MainDocumentPart);
        }

        document.Save();
        document.Close();

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