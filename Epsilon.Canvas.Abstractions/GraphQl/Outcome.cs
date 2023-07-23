using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.GraphQl;

public record Outcome(
    [property: JsonPropertyName("_id")] int Id
);