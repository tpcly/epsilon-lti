using System.Diagnostics.CodeAnalysis;

namespace Epsilon.Abstractions.Services;

public interface ICompetenceDocumentService
{
    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Suppressed for later implementation. Remove when implemented ")]
    Task<CompetenceDocument> GetDocument(int courseId, DateTime from, DateTime to);

    void WriteDocument(Stream stream, CompetenceDocument document);
}