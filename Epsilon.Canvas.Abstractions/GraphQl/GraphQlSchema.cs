using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.GraphQl;

public record GraphQlSchema(
    [property: JsonPropertyName("allCourses")] IEnumerable<Course>? Courses,
    [property: JsonPropertyName("course")] Course? Course
);