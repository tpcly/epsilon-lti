namespace Epsilon.Abstractions.Services;

public interface ICompetenceDocumentService
{
    // ReSharper disable once UnusedParameter.Global
    // ReSharper disable once UnusedParameter.Global
    Task<CompetenceDocument> GetDocument(int courseId, DateTime from, DateTime to);

    void WriteDocument(Stream stream, CompetenceDocument document);
}