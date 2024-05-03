using System.Globalization;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Tpcly.Canvas.Abstractions.GraphQl;

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

    public FilterService(ICanvasUserSessionAccessor sessionAccessor, ICanvasGraphQlApi canvasGraphQl)
    {
        _sessionAccessor = sessionAccessor;
        _canvasGraphQl = canvasGraphQl;
    }

    public async Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms(string studentId)
    {
        var canvasUser = await _sessionAccessor.GetSessionAsync();
        var variables = new Dictionary<string, object> { { "studentIds", studentId }, };
        var response = await _canvasGraphQl.Query(ParticipatedTermQuery, variables);

        if (response?.LegacyNode?.Enrollments == null)
        {
            return Enumerable.Empty<EnrollmentTerm>();
        }
        
        //Get the term max based on the current course Epsilon resides in.
        var currentCourseEnrolment = response.LegacyNode.Enrollments.FirstOrDefault(enrollment => enrollment?.Course?.Id == canvasUser?.CourseId.ToString(CultureInfo.InvariantCulture));
        
        
        var participatedTerms = response.LegacyNode.Enrollments
                                        .Select(static e => e.Course?.Term!)
                                        .DistinctBy(static t => t?.Id)
                                        .Where(term => term is { StartAt: not null, EndAt: not null, } && term.StartAt <= (currentCourseEnrolment?.Course?.Term?.StartAt ?? DateTime.Now))
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

                return er.User?.LegacyId == canvasUser?.UserId.ToString(CultureInfo.InvariantCulture) && er.Type == "StudentEnrollment";
            }
        ).Select(static er => er.User!).DistinctBy(static u => u.LegacyId).OrderBy(static u => u.Name);
    }
}