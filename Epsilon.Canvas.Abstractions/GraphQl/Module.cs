using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.GraphQl;

public record Module(
    [property: JsonPropertyName("name")] string Name
);