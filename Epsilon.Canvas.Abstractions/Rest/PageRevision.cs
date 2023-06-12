using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Rest;

public record PageRevision(
    [property: JsonPropertyName("revision_id")] string? RevisionId,
    [property: JsonPropertyName("updated_at")] DateTime? UpdatedAt,
    [property: JsonPropertyName("latest")] bool? Latest
);