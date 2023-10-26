using System.ComponentModel.DataAnnotations;
using Tpcly.Persistence;

namespace Epsilon.Abstractions;

public record LearningDomainTypeSet : Entity<Guid>
{
    // ReSharper disable once ReplaceAutoPropertyWithComputedProperty
    [Required]
    public IEnumerable<LearningDomainType> Types { get; private set; } = null!;

    public void Order()
    {
        Types = Types.OrderBy(static t => t.Order);
    }
}