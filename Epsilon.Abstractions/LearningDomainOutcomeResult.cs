using System.ComponentModel.DataAnnotations;

namespace Epsilon.Abstractions;

public record LearningDomainOutcomeResult(
    [Required]
    LearningDomainOutcome Outcome,
    double? Grade
);