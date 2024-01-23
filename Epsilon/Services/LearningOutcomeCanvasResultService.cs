using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Tpcly.Canvas.Abstractions.GraphQl;

namespace Epsilon.Services;

public class LearningOutcomeCanvasResultService : ILearningOutcomeCanvasResultService
{
    private const string Query = @"
query GetSubmissions($studentIds: ID!) {
  legacyNode(_id: $studentIds, type: User) {
    ... on User {
      enrollments {
        _id
        course {
          _id
          name
          submissionsConnection(studentIds: [$studentIds]) {
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
        var submissionsTask = await _canvasGraphQlApi.Query(Query, new Dictionary<string, object> { { "studentIds", studentId }, });
        var domainOutcomesTask = _learningDomainService.GetOutcomes();

        await Task.WhenAll(domainOutcomesTask, domainOutcomesTask);

        if (submissionsTask?.LegacyNode?.Enrollments == null)
        {
            throw new HttpRequestException("No Enrollments are given");
        }

        foreach (var enrollment in submissionsTask.LegacyNode.Enrollments.DistinctBy(static e => e.Course?.Id))
        {
            if (enrollment.Course?.Submissions?.Nodes != null)
            {
                foreach (var submissions in enrollment.Course.Submissions.Nodes.GroupBy(static s => s.Assignment?.HtmlUrl))
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

    private static IEnumerable<LearningDomainCriteria> GetSubmissionCriteria(Submission? submission, Task<IEnumerable<LearningDomainOutcome?>>? domainOutcomesTask)
    {
        if (submission!.Assignment?.Rubric?.Criteria != null)
        {
            foreach (var criteria in submission.Assignment.Rubric.Criteria)
            {
                if (criteria.Outcome != null)
                {
                    var existingDomainCriteria = domainOutcomesTask?.Result.SingleOrDefault(o => o?.Id == criteria.Outcome.Id) != null;
                    if (existingDomainCriteria)
                    {
                        yield return new LearningDomainCriteria(
                            criteria.Outcome.Id,
                            criteria.Outcome.MasteryPoints
                        );
                    }
                }
            }
        }
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
                sH.RubricAssessments?.Nodes.SelectMany(static ar => ar.AssessmentRatings.Where(static ar => ar is
                {
                    Points: not null,
                    Criterion.MasteryPoints: not null,
                    Criterion.Outcome: not null,
                })) ?? throw new HttpRequestException("Criteria for RubricAssessments not possible"));

            if (rubricAssessments == null)
            {
                throw new HttpRequestException("No RubricAssessments are found");
            }

            foreach (var assessment in rubricAssessments)
            {
                var outcome = domainOutcomesTask?.Result.SingleOrDefault(o => o?.Id == assessment?.Criterion?.Outcome?.Id);
                if (outcome != null)
                {
                    if (outcomeRecords.All(r => r.Outcome.Id != outcome.Id))
                    {
                        outcomeRecords.Add(new LearningDomainOutcomeRecord(outcome, assessment?.Points));
                    }
                }
            }
        }


        return outcomeRecords;
    }
}