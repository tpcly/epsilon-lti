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
            yield return new LearningDomainSubmission(
                submission.Assignment.Name,
                submission.Assignment.HtmlUrl,
                submission.SubmissionHistories.Nodes.OrderBy(static h => h.Attempt).First().SubmittedAt,
                GetSubmissionCriteria(submission),
                GetOutcomeResults(submission, domainOutcomesTask)
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

    
    private static Dictionary<int, LearningDomainOutcomeRecord> GetOutcomeResults(
        Submission submission,
        Task<IEnumerable<LearningDomainOutcome?>>? domainOutcomesTask
    )
    {
        var outcomeRecords = new Dictionary<int, LearningDomainOutcomeRecord>();
        //Loop trough all submissions of assignment to look for outcomes that have been graded. 
        foreach (var submissionHistory in submission.SubmissionHistories.Nodes.OrderByDescending(static s => s.SubmittedAt))
        {
            var rubricAssessments = submissionHistory.RubricAssessments.Nodes.SelectMany(static rubricAssessment =>
                rubricAssessment.AssessmentRatings.Where(static ar =>
                    ar is
                    {
                        Points: not null,
                        Criterion.MasteryPoints: not null,
                        Criterion.Outcome: not null,
                    }));

            foreach (var assessment in rubricAssessments)
            {
                var outcome = domainOutcomesTask?.Result.SingleOrDefault(o => o?.Id == assessment?.Criterion?.Outcome?.Id);
                if (outcome != null)
                {
                    //The latest graded outcome will be remaining. 
                    outcomeRecords.Remove(outcome.Id);
                    outcomeRecords.Add(outcome.Id, new LearningDomainOutcomeRecord(outcome, assessment?.Points));
                }
            }
        }

        return outcomeRecords;
    }
}