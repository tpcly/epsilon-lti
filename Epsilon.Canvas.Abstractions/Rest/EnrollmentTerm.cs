using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Rest;

public record EnrollmentTerm(
    [property: JsonPropertyName("name"), Required,] string Name,
    [property: JsonPropertyName("start_at")] DateTime? StartAt,
    [property: JsonPropertyName("end_at")] DateTime? EndAt
);