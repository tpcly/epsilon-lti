using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.GraphQl;

public record Course(
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("submissionsConnection")] GraphQlConnection<Submission>? Submissions,
    [property: JsonPropertyName("enrollmentsConnection")] GraphQlConnection<Enrollment>? Enrollments
);