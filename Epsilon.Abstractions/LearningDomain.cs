using System.ComponentModel.DataAnnotations;
using Tpcly.Persistence;


namespace Epsilon.Abstractions;

public record LearningDomain : Entity<string>
{
    [Required]
    public LearningDomainTypeSet RowsSet { get; set; } = null!;
    public LearningDomainTypeSet? ColumnsSet { get; set; }
    [Required]
    public LearningDomainTypeSet ValuesSet { get; set; } = null!;
}