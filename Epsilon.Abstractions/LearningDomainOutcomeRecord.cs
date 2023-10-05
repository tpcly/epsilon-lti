using System.ComponentModel.DataAnnotations;

namespace Epsilon.Abstractions;

public record LearningDomainOutcomeRecord(
    [Required]
    LearningDomainOutcome Outcome,
    double? Grade
);