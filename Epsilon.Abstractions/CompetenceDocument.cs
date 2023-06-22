using Epsilon.Abstractions.Components;

namespace Epsilon.Abstractions;

public record CompetenceDocument(IEnumerable<IWordCompetenceComponent> Components);