using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.GraphQl;

public record GraphQlQueryResponse(
    [property: JsonPropertyName("data")] GraphQlSchema? Data
);