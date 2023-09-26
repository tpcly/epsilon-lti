namespace Epsilon.Abstractions.Services;

public interface ICompetenceDocumentService
{
    Task<CompetenceDocument> GetDocument(int courseId, string userId, DateTime from, DateTime to);

    void WriteDocument(Stream stream, CompetenceDocument document);
}