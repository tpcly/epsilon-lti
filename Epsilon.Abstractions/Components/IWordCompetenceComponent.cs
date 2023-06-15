using DocumentFormat.OpenXml.Packaging;

namespace Epsilon.Abstractions.Components;

public interface IWordCompetenceComponent
{
    public void AddToWordDocument(MainDocumentPart mainDocumentPart);
}