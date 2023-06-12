namespace Epsilon.Canvas.Abstractions;

/// <summary>
/// Canvas user session model which holds necessary information to keep track of a Canvas user. Still prone to change.
/// </summary>
/// <param name="CourseId">Canvas personal course id</param>
/// <param name="StudentId">Canvas student id</param>
/// <param name="AccessToken">Canvas access token</param>
public record CanvasUserSession(
    int CourseId,
    int StudentId,
    string AccessToken
);