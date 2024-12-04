using Epsilon.Abstractions.Components;

namespace Epsilon.Abstractions;

public record SupplementDocument(IAsyncEnumerable<IWordCompetenceComponent> Components);