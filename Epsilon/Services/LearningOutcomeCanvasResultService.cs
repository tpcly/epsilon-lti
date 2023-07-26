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
                  rubric {
                    criteria {
                      outcome {
                        _id
                        masteryPoints
                      }
                    }
                  }
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

    public async IAsyncEnumerable<LearningDomainSubmission> GetSubmissions(string studentId)
    {
        var submissionsTask = _canvasGraphQlApi.Query(Query, new Dictionary<string, object> { { "studentIds", studentId }, });
        var domainOutcomesTask = _learningDomainService.GetOutcomes();

        await Task.WhenAll(submissionsTask, domainOutcomesTask);

        foreach (var submission in submissionsTask?.Result?.Courses?.SelectMany(static c => c.Submissions.Nodes))
        {
            var recentSubmission = submission.SubmissionHistories?.Nodes
                                             .MaxBy(static h => h.Attempt);
            

            var rubricAssessments = recentSubmission.RubricAssessments.Nodes.SelectMany(static rubricAssessment =>
                rubricAssessment.AssessmentRatings.Where(static ar =>
                    ar is
                    {
                        Points: not null,
                        Criterion.MasteryPoints: not null,
                        Criterion.Outcome: not null,
                    })
            );
 
            yield return new LearningDomainSubmission(
                submission.Assignment.Name,
                submission.Assignment.HtmlUrl,
                recentSubmission.SubmittedAt,
                GetSubmissionCriteria(submission),
                GetOutcomeResults(rubricAssessments, domainOutcomesTask)
            );
        }
    }

    private static IEnumerable<LearningDomainCriteria> GetSubmissionCriteria(Submission? submission)
    {
        if (submission!.Assignment?.Rubric?.Criteria != null)
        {
            foreach (var criteria in submission.Assignment.Rubric.Criteria)
            {
                if (criteria.Outcome != null)
                {
                    yield return new LearningDomainCriteria(
                        criteria.Outcome.Id,
                        criteria.Outcome.MasteryPoints
                    );
                }
            }
        }
    }
    
    private static IEnumerable<LearningDomainOutcomeResult> GetOutcomeResults(
        IEnumerable<AssessmentRating> rubricAssessments,
        Task<IEnumerable<LearningDomainOutcome?>>? domainOutcomesTask
    )
    {
        foreach (var assessmentRating in rubricAssessments)
        {
            var outcome = domainOutcomesTask?.Result.SingleOrDefault(o => o.Id == assessmentRating.Criterion.Outcome.Id);
            if (outcome == null)
            {
                continue;
            }
    
            yield return new LearningDomainOutcomeResult(outcome, assessmentRating.Points);
        }
    }
}