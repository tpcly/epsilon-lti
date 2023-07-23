using NetCore.Persistence;

namespace Epsilon.Abstractions;

public record LearningDomain : Entity<string>
{
    public LearningDomainTypeSet RowsSet { get; set; } = null!;
    public LearningDomainTypeSet? ColumnsSet { get; set; }
    public LearningDomainTypeSet ValuesSet { get; set; } = null!;
}