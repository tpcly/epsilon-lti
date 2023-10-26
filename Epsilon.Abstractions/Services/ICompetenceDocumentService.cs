namespace Epsilon.Abstractions.Services;

public interface ICompetenceDocumentService
{
    Task<CompetenceDocument> GetDocument(int courseId, string userId, DateTime? from = null, DateTime? to = null);

    void WriteDocument(Stream stream, CompetenceDocument document);
}