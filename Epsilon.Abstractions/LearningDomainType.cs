using System.Text.Json.Serialization;
using Tpcly.Persistence;

namespace Epsilon.Abstractions;

public record LearningDomainType : Entity<string>
{
    public string Name { get; set; } = null!;
    public string ShortName { get; set; } = null!;
    public string? HexColor { get; set; }
    
    [JsonIgnore]
    public IEnumerable<LearningDomainTypeSet>? Sets { get; set; }
}