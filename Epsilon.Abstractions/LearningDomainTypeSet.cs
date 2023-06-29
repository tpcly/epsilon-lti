using Epsilon.Abstractions.Data;

namespace Epsilon.Abstractions;

public record LearningDomainTypeSet : Entity<Guid>
{
    public IEnumerable<LearningDomainType> Types { get; } = null!;
}