using System.Globalization;
using Epsilon.Abstractions.Services;
using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.GraphQl;
using Epsilon.Canvas.Abstractions.Rest;

namespace Epsilon.Services;

public class FilterService : IFilterService
{
    private const string ParticipatedTermQuery = @"
        query GetUserSubmissions($studentIds: [ID!]) {
          allCourses {
            submissionsConnection(studentIds: $studentIds) {
              nodes {
                submittedAt
              }
            }
          }
        }
    ";

    private const string AccessibleEnrollmentsQuery = @"
        query AccessibleEnrollments {
          allCourses {
            enrollmentsConnection(filter: {types: [TeacherEnrollment, StudentEnrollment]}) {
              nodes {
                type
                user {
                  name
                  avatarUrl
                  _id
                }
              }
            }
          }
        }
    ";

    private readonly CanvasUserSession _canvasUser;
    private readonly ICanvasGraphQlApi _canvasGraphQl;
    private readonly ICanvasRestApi _canvasRest;

    public FilterService(CanvasUserSession canvasUser, ICanvasGraphQlApi canvasGraphQl, ICanvasRestApi canvasRest)
    {
        _canvasUser = canvasUser;
        _canvasGraphQl = canvasGraphQl;
        _canvasRest = canvasRest;
    }

    public async Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms(string studentId)
    {
        var allTerms = await _canvasRest.Accounts.GetTerms(1);

        var variables = new Dictionary<string, object> { { "studentIds", studentId }, };
        var response = await _canvasGraphQl.Query(ParticipatedTermQuery, variables);

        if (response == null)
        {
            return Enumerable.Empty<EnrollmentTerm>();
        }

        var submissions = response.Courses!.SelectMany(static c => c.Submissions!.Nodes);

        var participatedTerms = allTerms!
                                .Where(static term => term is { StartAt: not null, EndAt: not null, })
                                .Where(term => submissions.Any(submission => submission.SubmittedAt >= term.StartAt && submission.SubmittedAt <= term.EndAt))
                                .Distinct()
                                .OrderByDescending(static term => term.StartAt);

        return participatedTerms;
    }

    // TODO: Has some issues due to the fact that it does not know whether the selected student has submissions or not
    public async Task<IEnumerable<User>> GetAccessibleStudents()
    {
        var response = await _canvasGraphQl.Query(AccessibleEnrollmentsQuery);

        return response?.Courses!
                       .Where(c => c.Enrollments != null && c.Enrollments.Nodes.Any(e =>
                               e.User.LegacyId == _canvasUser.StudentId.ToString(CultureInfo.InvariantCulture)
                               && e.Type == "TeacherEnrollment"
                           )
                       )
                       .SelectMany(static c =>
                           c.Enrollments!.Nodes.Where(static e => e.Type == "StudentEnrollment")
                            .Select(static s => s.User)
                       )
                       .Distinct()
               ?? Array.Empty<User>();
    }
}