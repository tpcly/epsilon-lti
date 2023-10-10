using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tpcly.Persistence;

namespace Epsilon.Abstractions;

public record LearningDomainType : Entity<string>
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string ShortName { get; set; } = null!;
    public string? HexColor { get; set; }
    public int Order { get; set; }
    
    [JsonIgnore]
    public IEnumerable<LearningDomainTypeSet>? Sets { get; set; }

    public LearningDomainType()
    {
        Order = 0;
    }
}