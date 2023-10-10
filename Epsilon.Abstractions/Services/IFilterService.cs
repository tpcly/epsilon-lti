using Epsilon.Canvas.Abstractions.GraphQl;
using Epsilon.Canvas.Abstractions.Rest;

namespace Epsilon.Abstractions.Services;

public interface IFilterService
{
    Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms(string studentId);
    Task<IEnumerable<User>> GetAccessibleStudents();
}



