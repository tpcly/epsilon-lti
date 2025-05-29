using System.Globalization;
using System.Net.Http.Json;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Tpcly.Canvas.Abstractions.GraphQl;
using Enrollment = Epsilon.Abstractions.Enrollment;
using EnrollmentTerm = Tpcly.Canvas.Abstractions.GraphQl.EnrollmentTerm;
using User = Tpcly.Canvas.Abstractions.Rest.User;

namespace Epsilon.Services;

public class FilterService : IFilterService
{
    private const string ParticipatedTermQuery = @"
        query GetUserTerms($studentIds: ID!) {
          legacyNode(_id: $studentIds, type: User) {
            ... on User {
              enrollments {
                course {
                  _id
                  term {
                    _id
                    name
                    startAt
                    endAt
                  }
                }
              }
            }
          }
        }

    ";

    private readonly ICanvasUserSessionAccessor _sessionAccessor;
    private readonly ICanvasGraphQlApi _canvasGraphQl;
    private readonly IEpsilonCanvasHttpClient _canvasRestApi;

    public FilterService(ICanvasUserSessionAccessor sessionAccessor, ICanvasGraphQlApi canvasGraphQl, IEpsilonCanvasHttpClient canvasRestApi)
    {
        _canvasRestApi = canvasRestApi;
        _sessionAccessor = sessionAccessor;
        _canvasGraphQl = canvasGraphQl;
    }

    public async Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms(string studentId)
    {
        var canvasUser = await _sessionAccessor.GetSessionAsync();
        var variables = new Dictionary<string, object> { { "studentIds", studentId }, };
        var response = await _canvasGraphQl.Query(ParticipatedTermQuery, variables);

        if (response?.LegacyNode?.Enrollments == null)
            return Enumerable.Empty<EnrollmentTerm>();

        //Get the term max based on the current course Epsilon resides in.
        var currentCourseEnrolment
            = response.LegacyNode.Enrollments.FirstOrDefault(enrollment => enrollment?.Course?.Id == canvasUser?.CourseId.ToString(CultureInfo.InvariantCulture));


        var participatedTerms = response.LegacyNode.Enrollments
                                        .Select(static e => e.Course?.Term!)
                                        .DistinctBy(static t => t?.Id)
                                        .Where(term => term is { StartAt: not null, EndAt: not null, }
                                                       && term.StartAt <= (currentCourseEnrolment?.Course?.Term?.StartAt ?? DateTime.Now))
                                        .OrderByDescending(static term => term?.StartAt).ToList();


        // Get the corrected term based on a new end date:
        var correctedParticipatedTerms = participatedTerms
                                         .Select((currentTerm, index) => currentTerm with
                                         {
                                             EndAt = index > 0
                                                 ? participatedTerms.ElementAt(index - 1)?.StartAt
                                                 : currentTerm.EndAt,
                                         })
                                         .ToList();


        return correctedParticipatedTerms;
    }


    public async Task<IEnumerable<User>> GetAccessibleStudents()
    {
        var canvasUser = await _sessionAccessor.GetSessionAsync();

        var response = _canvasRestApi.Request(HttpMethod.Get, $"https://fhict.instructure.com/api/v1/courses/{canvasUser!.CourseId}/enrollments?per_page=100");
        var responseContent = await response.Result.Content.ReadFromJsonAsync<IEnumerable<Enrollment>>();
        if (response.Result.Headers.Contains("link"))
        {
            var url = response.Result.Headers.GetValues("link").First();
            var nextUrl = GetNextLinkUrl(url);
            do
            {
                if (nextUrl != null)
                {
                    var responseb = _canvasRestApi.Request(HttpMethod.Get, nextUrl);
                    var results = await responseb.Result.Content.ReadFromJsonAsync<IEnumerable<Enrollment>>();
                    responseContent = responseContent!.Concat(results ?? []);

                    nextUrl = GetNextLinkUrl(responseb.Result.Headers.GetValues("link").First());
                }
            }
            while (nextUrl != null);
        }
        
        return  responseContent.Where(er =>
            {
                if (canvasUser?.IsTeacher ?? false)
                    return er.Type == "StudentEnrollment";

                return er.User.Id == canvasUser?.UserId && er.Type == "StudentEnrollment";
            }
        ).Select(static er => er.User).DistinctBy(static u => u.Id).OrderBy(static u => u.Name)
            .ToList();
    }


    private static string? GetNextLinkUrl(string linkHeader)
    {
        var links = linkHeader.Split(',');
        foreach (var link in links)
        {
            var parts = link.Split(';');
            if (parts.Length < 2) continue;

            var urlPart = parts[0].Trim();
            var relPart = parts[1].Trim();

            if (relPart.Equals("rel=\"next\"", StringComparison.OrdinalIgnoreCase))
            {
                return urlPart.Trim('<', '>');
            }
        }

        return null;
    }
}