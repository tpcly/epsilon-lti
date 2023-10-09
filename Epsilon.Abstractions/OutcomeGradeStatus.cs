using System.Diagnostics.CodeAnalysis;

namespace Epsilon.Abstractions;

[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Suppressed for later implementation. Remove when implemented ")]
public enum OutcomeGradeStatus
{
    Mastered,
    NotMastered,
    NotGraded,
    NotAssessed,
}