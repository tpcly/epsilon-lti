using System.Diagnostics.CodeAnalysis;

namespace Epsilon.Abstractions;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public enum OutcomeGradeStatus
{
    Mastered,
    NotMastered,
    NotGraded,
    NotAssessed,
}