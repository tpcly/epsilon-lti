using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Rest;

public record EnrollmentTermCollection(
    [property: JsonPropertyName("enrollment_terms"), Required] IEnumerable<EnrollmentTerm> Terms
);