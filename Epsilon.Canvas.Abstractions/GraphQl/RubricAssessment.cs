using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.GraphQl;

public record RubricAssessment(
    [property: JsonPropertyName("assessmentRatings")] IReadOnlyList<AssessmentRating?>? AssessmentRatings
);