using System.Globalization;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Tpcly.Canvas.Abstractions.GraphQl;
using Tpcly.Canvas.Abstractions.Rest;
using User = Tpcly.Canvas.Abstractions.Rest.User;

namespace Epsilon.Services;

public class LearningOutcomeCanvasResultService : ILearningOutcomeCanvasResultService
{
    private const string Query = @"
        query GetSubmissions($studentIds: ID!, $submittedSince: DateTime) {
          legacyNode(_id: $studentIds, type: User) {
            ... on User {
              enrollments {
                _id
                course {
                  _id
                  name
                  submissionsConnection(studentIds: [$studentIds],  filter: {submittedSince: $submittedSince}, first: 100) {
                    nodes {
                      assignment {
                        _id
                        htmlUrl
                        name
                        rubric {
                          criteria {
                            outcome {
                              _id
                              masteryPoints
                            }
                          }
                        }
                      }
                      submissionHistoriesConnection {
                        nodes {
                          attempt
                          submittedAt
                          postedAt
                          rubricAssessmentsConnection {
                            nodes {
                              assessmentRatings {
                                _id
                                points
                                criterion {
                                  outcome {
                                    _id
                                    title
                                    masteryPoints
                                  }
                                  masteryPoints
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
    ";

    private readonly ICanvasGraphQlApi _canvasGraphQlApi;
    private readonly ICanvasRestApi _canvasRestApi;
    private readonly ILearningDomainService _learningDomainService;

    public LearningOutcomeCanvasResultService(
        ICanvasGraphQlApi canvasGraphQlApi,
        ICanvasRestApi canvasRestApi,
        ILearningDomainService learningDomainService
    )
    {
        _canvasGraphQlApi = canvasGraphQlApi;
        _canvasRestApi = canvasRestApi;
        _learningDomainService = learningDomainService;
    }

    public async IAsyncEnumerable<LearningDomainSubmission> GetSubmissions(string studentId, DateTime? submittedSince = null)
    {
        var submissionsTask = _canvasGraphQlApi.Query(Query, new Dictionary<string, object>
        {
            { "studentIds", studentId },
            { "submittedSince", submittedSince?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? "1970-01-01" },
        });
        
        var domainOutcomesTask = _learningDomainService.GetOutcomes();

        await Task.WhenAll(submissionsTask, domainOutcomesTask);

        var enrollments = submissionsTask.Result?.LegacyNode?.Enrollments ?? throw new HttpRequestException("No Enrollments are given");

        foreach (var enrollment in enrollments.DistinctBy(static e => e.Course?.Id))
        {
            if (enrollment.Course?.Submissions?.Nodes != null)
            {
                foreach (var submissions in enrollment.Course.Submissions.Nodes.GroupBy(static s => s.Assignment?.HtmlUrl ))
                {
                    var latestSubmission = submissions.First();

                    yield return new LearningDomainSubmission(
                        latestSubmission.Assignment?.Name,
                        latestSubmission.Assignment?.HtmlUrl,
                        latestSubmission.SubmissionHistories?.Nodes.OrderByDescending(static sh => sh.SubmittedAt).First().SubmittedAt ?? new DateTime(),
                        GetSubmissionCriteria(latestSubmission, domainOutcomesTask),
                        GetOutcomeResults(submissions, domainOutcomesTask)
                    );
                }
            }
        }
    }

    public async Task<IEnumerable<User>?> SearchUsers(int accountId, string query)
    {
        return await _canvasRestApi.Accounts.GetUsers(accountId, query);
    }

    private static IEnumerable<LearningDomainCriteria> GetSubmissionCriteria(Submission? submission, Task<IEnumerable<LearningDomainOutcome?>>? domainOutcomesTask)
    {
        var results = new List<LearningDomainCriteria>();
        if (submission!.Assignment?.Rubric?.Criteria != null)
        {
            foreach (var criteria in submission.Assignment.Rubric.Criteria)
            {
                if (criteria.Outcome != null)
                {
                    var existingDomainCriteria = domainOutcomesTask?.Result.SingleOrDefault(o => o?.Id == criteria.Outcome.Id) != null;
                    if (existingDomainCriteria)
                    {
                        results.Add(new LearningDomainCriteria(
                            criteria.Outcome.Id,
                            criteria.Outcome.MasteryPoints
                        ));
                    }
                }
            }
        }

        return results;
    }


    private static IEnumerable<LearningDomainOutcomeRecord> GetOutcomeResults(
        IEnumerable<Submission> submissions,
        Task<IEnumerable<LearningDomainOutcome?>>? domainOutcomesTask
    )
    {
        var outcomeRecords = new List<LearningDomainOutcomeRecord>();

        foreach (var submissionHistory in submissions.OrderByDescending(static s => s.SubmittedAt))
        {
            var rubricAssessments = submissionHistory.SubmissionHistories?.Nodes.SelectMany(static sH =>
                sH.RubricAssessments?.Nodes.SelectMany(static ar => ar.AssessmentRatings?.Where(static ar => ar is
                {
                    Points: not null,
                    Criterion.MasteryPoints: not null,
                    Criterion.Outcome: not null,
                }) ?? throw new HttpRequestException("Assessment Ratings not possible for rubric assessment"))
                ?? throw new HttpRequestException("Criteria for RubricAssessments not possible")) 
                ?? throw new HttpRequestException("RubricAssessments not possible");

            foreach (var assessment in rubricAssessments)
            {
                var outcome = domainOutcomesTask?.Result.SingleOrDefault(o => o?.Id == assessment?.Criterion?.Outcome?.Id);
                if (outcome != null)
                {
                    if (outcomeRecords.All(r => r.Outcome.Id != outcome.Id))
                        outcomeRecords.Add(new LearningDomainOutcomeRecord(outcome, assessment?.Points));
                }
            }
        }


        return outcomeRecords;
    }
    
    
}