namespace Epsilon.Abstractions.Services;

public interface ICompetenceDocumentService
{
    CompetenceDocument GetDocument(string userId, DateTime? from = null, DateTime? to = null);

    static Task<bool> WriteDocument(Stream stream, CompetenceDocument document)
    {
        throw new NotImplementedException();
    }
}