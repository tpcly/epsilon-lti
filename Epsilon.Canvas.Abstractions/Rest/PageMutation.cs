using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Rest;

public record PageMutation
{
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("body")]
    public string? Body { get; init; }
}