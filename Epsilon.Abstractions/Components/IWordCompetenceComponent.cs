using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Components;

public interface IWordCompetenceComponent
{
    // ReSharper disable once UnusedMemberInSuper.Global
    public Task<Body> AddToWordDocument(MainDocumentPart mainDocumentPart);
}