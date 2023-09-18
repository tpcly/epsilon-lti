using Tpcly.Persistence;

namespace Epsilon.Abstractions;

public record LearningDomainTypeSet : Entity<Guid>
{
    // ReSharper disable once ReplaceAutoPropertyWithComputedProperty
    public IEnumerable<LearningDomainType> Types { get; } = null!;
}