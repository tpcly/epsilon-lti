namespace Epsilon.Components;

public record KpiMatrixOutcome(
    int Id,
    string Title,
    KpiMatrixOutcomeGradeStatus GradeStatus
);