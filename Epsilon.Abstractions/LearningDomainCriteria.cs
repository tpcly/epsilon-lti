using System.ComponentModel.DataAnnotations;

namespace Epsilon.Abstractions;

public record LearningDomainCriteria(
    [Required]
    int Id,
    double? MasteryPoints
);