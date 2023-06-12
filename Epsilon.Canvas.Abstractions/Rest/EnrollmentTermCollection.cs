using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Rest;

public record EnrollmentTermCollection(
    [property: JsonPropertyName("enrollment_terms")] IEnumerable<EnrollmentTerm> Terms
);