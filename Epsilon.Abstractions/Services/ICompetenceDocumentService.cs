namespace Epsilon.Abstractions.Services;

public interface ICompetenceDocumentService
{
    Task<Stream> WriteDocument(Stream stream, DateTime from, DateTime to);
}