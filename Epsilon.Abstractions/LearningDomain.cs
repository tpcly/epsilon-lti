using System.ComponentModel.DataAnnotations;
using Tpcly.Persistence;


namespace Epsilon.Abstractions;

public record LearningDomain : Entity<string>
{
    [Required]
    // ReSharper disable once MemberCanBePrivate.Global
    public LearningDomainTypeSet RowsSet { get; set; } = null!;
    public LearningDomainTypeSet? ColumnsSet { get; set; }
    [Required]
    // ReSharper disable once MemberCanBePrivate.Global
    public LearningDomainTypeSet ValuesSet { get; set; } = null!;
    
    public void Order()
    {
        RowsSet.Order();
        ColumnsSet?.Order();
        ValuesSet.Order();
    }
}