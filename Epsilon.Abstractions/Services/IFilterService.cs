using Tpcly.Canvas.Abstractions.GraphQl;

namespace Epsilon.Abstractions.Services;

public interface IFilterService
{
    Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms(string studentId);
    Task<IEnumerable<User>?> GetAccessibleStudents();
}



