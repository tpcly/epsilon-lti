using System.Diagnostics.CodeAnalysis;

namespace Epsilon.Abstractions.Services;

public interface ICompetenceDocumentService
{
    //Suppressed for later implementation. Remove when implemented  
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    Task<CompetenceDocument> GetDocument(int courseId, DateTime from, DateTime to);

    void WriteDocument(Stream stream, CompetenceDocument document);
}