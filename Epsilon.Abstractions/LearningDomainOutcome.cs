using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tpcly.Persistence;

namespace Epsilon.Abstractions;

public record LearningDomainOutcome : Entity<int>
{
    [JsonIgnore]
    [Required]
    public Guid TenantId { get; set; }

    [Required]
    public LearningDomainType Row { get; set; } = null!;
    public LearningDomainType? Column { get; set; }
    [Required]
    public LearningDomainType Value { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public LearningDomain Domain { get; set; } = null!;
}