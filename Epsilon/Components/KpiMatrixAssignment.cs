namespace Epsilon.Components;

public record KpiMatrixAssignment(
    string Name,
    IEnumerable<KpiMatrixOutcome> Outcomes
);