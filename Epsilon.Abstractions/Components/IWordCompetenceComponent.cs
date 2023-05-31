using DocumentFormat.OpenXml.Packaging;

namespace Epsilon.Abstractions.Components;

public interface IWordCompetenceComponent : ICompetenceComponent
{
    public void AddToWordDocument(MainDocumentPart mainDocumentPart);
}