using System.Diagnostics.CodeAnalysis;

namespace Epsilon.Abstractions;

//Suppressed for later implementation. Remove when implemented  
[SuppressMessage("ReSharper", "UnusedType.Global")]
public enum OutcomeGradeStatus
{
    Mastered,
    NotMastered,
    NotGraded,
    NotAssessed,
}