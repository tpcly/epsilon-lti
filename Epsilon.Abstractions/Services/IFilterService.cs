using Epsilon.Canvas.Abstractions.Rest;

namespace Epsilon.Abstractions.Services;

public interface IFilterService
{
    Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms();
}