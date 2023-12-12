using Epsilon.Abstractions.Components;

namespace Epsilon.Abstractions;

public record CompetenceDocument(IAsyncEnumerable<AbstractCompetenceComponent> Components);