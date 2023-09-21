using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.GraphQl;

public record Outcome(
    [property: JsonPropertyName("_id"), Required] int Id,
    [property: JsonPropertyName("masteryPoints")] double? MasteryPoints
);