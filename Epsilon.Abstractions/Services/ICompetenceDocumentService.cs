using DocumentFormat.OpenXml.Packaging;

namespace Epsilon.Abstractions.Services;

public interface ICompetenceDocumentService
{
    Task<CompetenceDocument> GetDocument(string userId, DateTime from, DateTime to);

    Task<WordprocessingDocument> WriteDocument(Stream stream, CompetenceDocument document);
}