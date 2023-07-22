namespace Epsilon.Abstractions;

public record LearningDomainOutcomeResult(
    LearningDomainOutcome Outcome,
    double Grade,
    DateTime SubmittedAt,
    string Assignment,
    Uri AssignmentUrl
);