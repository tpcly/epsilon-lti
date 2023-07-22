using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Epsilon.Canvas.Abstractions.GraphQl;

namespace Epsilon.Services;

public class LearningOutcomeCanvasResultService : ILearningOutcomeCanvasResultService
{
    private const string Query = @"
        query GetSubmissions($studentIds: [ID!]) {
          allCourses {
            submissionsConnection(studentIds: $studentIds) {
              nodes {
                submissionHistoriesConnection {
                  nodes {
                    rubricAssessmentsConnection {
                      nodes {
                        assessmentRatings {
                          criterion {
                            outcome {
                              _id
                            }
                            masteryPoints
                          }
                          points
                        }
                      }
                    }
                    attempt
                    submittedAt
                  }
                }
                assignment {
                  name
                  htmlUrl
                }
                postedAt
              }
            }
          }
        }
    ";

    private readonly ICanvasGraphQlApi _canvasGraphQlApi;
    private readonly ILearningDomainService _learningDomainService;

    public LearningOutcomeCanvasResultService(
        ICanvasGraphQlApi canvasGraphQlApi,
        ILearningDomainService learningDomainService
    )
    {
        _canvasGraphQlApi = canvasGraphQlApi;
        _learningDomainService = learningDomainService;
    }

    public async IAsyncEnumerable<LearningDomainOutcomeResult> GetOutcomeResults(string studentId, DateTime startDate, DateTime endDate)
    {
        var submissionsTask = _canvasGraphQlApi.Query(Query, new Dictionary<string, object> { { "studentIds", studentId }, });
        var domainOutcomesTask = _learningDomainService.GetOutcomes();

        await Task.WhenAll(submissionsTask, domainOutcomesTask);

        foreach (var submission in submissionsTask.Result.Courses.SelectMany(static c => c.Submissions.Nodes))
        {
            var recentSubmission = submission.SubmissionHistories?.Nodes
                                             .Where(sub => sub.SubmittedAt > startDate && sub.SubmittedAt < endDate)
                                             .Where(static h => h.RubricAssessments != null && h.RubricAssessments.Nodes.Any())
                                             .MaxBy(static h => h.Attempt);

            if (recentSubmission?.RubricAssessments?.Nodes == null)
            {
                continue;
            }

            var rubricAssessments = recentSubmission.RubricAssessments.Nodes.SelectMany(static rubricAssessment =>
                rubricAssessment.AssessmentRatings.Where(static ar =>
                    ar is
                    {
                        Points: not null,
                        Criterion.MasteryPoints: not null,
                        Criterion.Outcome: not null,
                    } &&
                    ar.Points >= ar.Criterion.MasteryPoints)
            );

            foreach (var assessmentRating in rubricAssessments)
            {
                var outcome = domainOutcomesTask.Result.SingleOrDefault(o => o.Id == assessmentRating.Criterion.Outcome.Id);
                if (outcome == null)
                {
                    continue;
                }

                yield return new LearningDomainOutcomeResult(
                    outcome,
                    assessmentRating.Points!.Value,
                    recentSubmission.SubmittedAt!.Value,
                    submission.Assignment.Name,
                    submission.Assignment.HtmlUrl
                );
            }
        }
    }
}