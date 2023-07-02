using System.Text.Json.Serialization;
using Epsilon.Abstractions.Data;

namespace Epsilon.Abstractions;

public record LearningDomainOutcome : Entity<int>
{
    [JsonIgnore]
    public Guid TenantId { get; set; }

    public LearningDomainType Row { get; set; } = null!;
    public LearningDomainType? Column { get; set; }
    public LearningDomainType Value { get; set; } = null!;
}