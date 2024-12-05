namespace Epsilon.Abstractions;

public record EdubadgeRecord
{
    public string Email { get; set; } = null!;
    public string Eppn { get; set; } = null!;
    public string Narrative { get; set; } = null!;
    public string EvidenceUrl { get; set; } = null!;
    public string EvidenceName { get; set; } = null!;
    public string EvidenceDescription { get; set; } = null!;
    public string Grade { get; set; } = null!;
};