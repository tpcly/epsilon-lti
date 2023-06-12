using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.GraphQl;

public record GraphQlConnection<TNode>(
    [property: JsonPropertyName("nodes")] IReadOnlyList<TNode> Nodes
);