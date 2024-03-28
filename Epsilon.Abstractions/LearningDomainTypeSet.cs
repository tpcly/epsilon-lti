using System.ComponentModel.DataAnnotations;
using Tpcly.Persistence;

namespace Epsilon.Abstractions;

public record LearningDomainTypeSet : Entity<Guid>
{
    [Required]
    public IEnumerable<LearningDomainType> Types { get; set; } = null!;

    public void Order()
    {
        Types = Types.OrderBy(static t => t.Order);
    }
}