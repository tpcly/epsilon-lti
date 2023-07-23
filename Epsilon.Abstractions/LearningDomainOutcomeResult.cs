namespace Epsilon.Abstractions;

public record LearningDomainOutcomeResult(
    LearningDomainOutcome Outcome,
    double Grade,
    OutcomeGradeStatus GradeStatus
);