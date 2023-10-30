using DocumentFormat.OpenXml.Packaging;

namespace Epsilon.Abstractions.Components;

public interface IWordCompetenceComponent
{
    // ReSharper disable once UnusedMemberInSuper.Global
    public void AddToWordDocument(MainDocumentPart mainDocumentPart);
}