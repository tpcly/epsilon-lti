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
}