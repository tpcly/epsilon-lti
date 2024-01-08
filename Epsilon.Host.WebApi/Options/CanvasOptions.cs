using System.ComponentModel.DataAnnotations;

namespace Epsilon.Host.WebApi.Options;

/// <summary>
/// A temporary mock class that is used to hold Canvas options,
/// should be removed after proper LTI support is added into the system, after which this becomes obsolete 
/// </summary>
public class CanvasOptions
{
    [Required]
    public Uri ApiUrl { get; set; } = null!;

    public int? OverrideCourseId { get; set; }

    public int? OverrideUserId { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string AccessToken { get; set; } = string.Empty;
}