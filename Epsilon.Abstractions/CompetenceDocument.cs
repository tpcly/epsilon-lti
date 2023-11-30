using Epsilon.Abstractions.Components;

namespace Epsilon.Abstractions;

public record CompetenceDocument(IEnumerable<AbstractCompetenceComponent> Components);