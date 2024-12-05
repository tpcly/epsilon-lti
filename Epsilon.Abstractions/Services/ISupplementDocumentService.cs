using DocumentFormat.OpenXml.Packaging;

namespace Epsilon.Abstractions.Services;

public interface ISupplementDocumentService
{
    Task<SupplementDocument> GetDocument(string userId, string[] domains);

    Task<WordprocessingDocument> WriteDocument(Stream stream, SupplementDocument document);
}