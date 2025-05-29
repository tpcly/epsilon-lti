using System.Text.Json.Serialization;
using Tpcly.Canvas.Abstractions.Rest;
namespace Epsilon.Abstractions;

public record Enrollment(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("user")] User User,
    [property: JsonPropertyName("type")] string Type
    );