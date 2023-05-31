using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.GraphQl;

public record Criteria(
    [property: JsonPropertyName("outcome")] Outcome? Outcome
);