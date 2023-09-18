using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using Epsilon.Abstractions.Services;

namespace Epsilon.Services;

public class CompetenceDocumentService : ICompetenceDocumentService
{
    private readonly IPageComponentManager _pageComponent;

    public CompetenceDocumentService(IPageComponentManager pageComponent)
    {
        _pageComponent = pageComponent;
    }

    public async Task<CompetenceDocument> GetDocument(int courseId, DateTime from, DateTime to)
    {
        var components = await FetchComponents(courseId).ToListAsync();

        return new CompetenceDocument(components);
    }

    public void WriteDocument(Stream stream, CompetenceDocument document)
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
    }

    private async IAsyncEnumerable<IWordCompetenceComponent> FetchComponents(int courseId)
    {
        yield return await _pageComponent.Fetch(courseId, "homepage");
        yield return await _pageComponent.Fetch(courseId, "projects");
    }
}