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
    private readonly IComponentManager _domainComponent;

    public CompetenceDocumentService(IPageComponentManager pageComponent, IComponentManager domainComponent)
    {
        _pageComponent = pageComponent;
        _domainComponent = domainComponent;
    }

    public async Task<CompetenceDocument> GetDocument(int courseId, string userId, DateTime from, DateTime to)
    {
        var components = await FetchComponents(courseId, userId).ToListAsync();

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

    private async IAsyncEnumerable<IWordCompetenceComponent> FetchComponents(int courseId, string userId)
    {
        yield return await _pageComponent.Fetch(courseId, "homepage", userId, "hbo-i-2018");
        yield return await _pageComponent.Fetch(courseId, "projects", userId, "hbo-i-2018");
        yield return await _domainComponent.Fetch(courseId, "domain", userId, "hbo-i-2018");
    }
}