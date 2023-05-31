using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.GraphQl;

public record Submission(
    [property: JsonPropertyName("postedAt")] DateTime? PostedAt,
    [property: JsonPropertyName("assignment")] Assignment? Assignment,
    [property: JsonPropertyName("submissionHistoriesConnection")] GraphQlConnection<SubmissionHistory>? SubmissionHistories,
    [property: JsonPropertyName("rubricAssessmentsConnection")] GraphQlConnection<RubricAssessment>? RubricAssessments
);