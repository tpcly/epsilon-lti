namespace Epsilon.Abstractions;

public record LearningDomainSubmission(
    string? Assignment,
    Uri? AssignmentUrl,
    DateTime? SubmittedAt,
    IEnumerable<LearningDomainCriteria>? Criteria,
    Dictionary<int, LearningDomainOutcomeRecord>? Results
);