using Tpcly.Canvas.Abstractions.GraphQl;
using User = Tpcly.Canvas.Abstractions.Rest.User;

namespace Epsilon.Abstractions.Services;

public interface IFilterService
{
    Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms(string studentId);
    Task<IEnumerable<User>> GetAccessibleStudents();
}



