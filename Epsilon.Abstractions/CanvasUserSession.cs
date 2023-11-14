using System.ComponentModel.DataAnnotations;

namespace Epsilon.Abstractions;

/// <summary>
/// Canvas user session model which holds necessary information to keep track of a Canvas user. Still prone to change.
/// </summary>
/// <param name="CourseId">Canvas personal course id</param>
/// <param name="UserId">Canvas user id</param>
public record CanvasUserSession(
    [Required] int CourseId,
    [Required] int UserId,
    [Required] bool IsTeacher
);