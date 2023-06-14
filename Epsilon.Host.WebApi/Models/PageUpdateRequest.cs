using System.Text.Json.Serialization;

namespace Epsilon.Host.WebApi.Models;

public record PageUpdateRequest(
    [property: JsonPropertyName("body")] string? Body
);