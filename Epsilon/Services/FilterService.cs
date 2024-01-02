using System.Globalization;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Tpcly.Canvas.Abstractions.GraphQl;
using Tpcly.Canvas.Abstractions.Rest;

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
        query AccessibleEnrollments($courseId: ID!) {
          course(id: $courseId) {
            enrollmentsConnection {
              nodes {
                _id
                user {
                  _id
                  name
                }
                type
              }
            }
          }
        }
    ";

    private readonly ICanvasUserSessionAccessor _sessionAccessor;
    private readonly ICanvasGraphQlApi _canvasGraphQl;
    private readonly ICanvasRestApi _canvasRest;

    public FilterService(ICanvasUserSessionAccessor sessionAccessor, ICanvasGraphQlApi canvasGraphQl, ICanvasRestApi canvasRest)
    {
        _sessionAccessor = sessionAccessor;
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
                                .OrderByDescending(static term => term.StartAt).ToList();

        // Get the corrected term based on a new end date:
        var correctedParticipatedTerms = participatedTerms
                                         .Select((currentTerm, index) => currentTerm with
                                         {
                                             EndAt = index > 0
                                                 ? participatedTerms[index - 1].StartAt
                                                 : currentTerm.EndAt,
                                         })
                                         .ToList();

        return correctedParticipatedTerms;
    }


    public async Task<IEnumerable<User>?> GetAccessibleStudents()
    {
        var canvasUser = await _sessionAccessor.GetSessionAsync();
        var variables = new Dictionary<string, object> { { "courseId", canvasUser?.CourseId ?? throw new HttpRequestException() }, };
        var response = await _canvasGraphQl.Query(AccessibleEnrollmentsQuery, variables);

        return response?.Course?.Enrollments?.Nodes.Where(er =>
            {
                if (canvasUser?.IsTeacher ?? false)
                {
                    return er.Type == "StudentEnrollment";
                }

                return er.User.LegacyId == canvasUser?.UserId.ToString(CultureInfo.InvariantCulture) && er.Type == "StudentEnrollment";
            }
        ).Select(static er => er.User).DistinctBy(static u => u.LegacyId) ?? Array.Empty<User>();
    }
}