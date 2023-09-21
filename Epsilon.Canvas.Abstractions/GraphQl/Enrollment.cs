using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.GraphQl;

public record Enrollment(
    [property: JsonPropertyName("type"), Required] string Type,
    [property: JsonPropertyName("user"), Required] User User
);