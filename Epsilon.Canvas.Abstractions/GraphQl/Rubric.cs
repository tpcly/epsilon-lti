using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.GraphQl;

public record Rubric(
    [property: JsonPropertyName("criteria")] IEnumerable<Criteria>? Criteria
);