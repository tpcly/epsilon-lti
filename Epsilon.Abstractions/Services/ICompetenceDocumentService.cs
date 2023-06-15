namespace Epsilon.Abstractions.Services;

public interface ICompetenceDocumentService
{
    Task<CompetenceDocument> GetDocument(DateTime from, DateTime to);

    Task<Stream> WriteDocument(Stream stream, CompetenceDocument document);
}