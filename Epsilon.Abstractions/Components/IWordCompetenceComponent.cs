using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Components;

public interface IWordCompetenceComponent
{
    public Task<Body?> AddToWordDocument(MainDocumentPart mainDocumentPart);
}