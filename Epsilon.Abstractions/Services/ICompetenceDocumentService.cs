namespace Epsilon.Abstractions.Services;

public interface ICompetenceDocumentService
{
    Task<CompetenceDocument> GetDocument(int courseId, DateTime from, DateTime to);

    void WriteDocument(Stream stream, CompetenceDocument document);
}