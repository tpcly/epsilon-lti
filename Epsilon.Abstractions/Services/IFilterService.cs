using Tpcly.Canvas.Abstractions.GraphQl;
using Tpcly.Canvas.Abstractions.Rest;

namespace Epsilon.Abstractions.Services;

public interface IFilterService
{
    Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms(string studentId);
    Task<IEnumerable<User>?> GetAccessibleStudents();
}



