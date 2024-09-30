using System.Collections.ObjectModel;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;

namespace Epsilon.Services;

public class EduBadgeService : IEduBadgeService
{
    private readonly ILearningOutcomeCanvasResultService _canvasResultService;
    private readonly ILearningDomainService _learningDomainService;

    public EduBadgeService(ILearningOutcomeCanvasResultService canvasResultService, ILearningDomainService learningDomainService)
    {
        _canvasResultService = canvasResultService;
        _learningDomainService = learningDomainService;
    }

    public async Task<Dictionary<string, List<LearningDomainSubmission>>> GetData(Collection<string> userIds, DateTime from, DateTime to)
    {
        var results = new Dictionary<string, List<LearningDomainSubmission>>();
        foreach (var userId in userIds)
        {
            results.Add(userId, await _canvasResultService.GetSubmissions(userId, from)
                                                          .Where(static e => e.Criteria.Any())
                                                          .ToListAsync()); 
        }

        return results;
    }

    public async Task<string> WriteDocument(Dictionary<string, List<LearningDomainSubmission>> data)
    {
        var fileContents = "";
        foreach (var userSubmissions in data)
        {
            var domainFromResults = await _learningDomainService.GetDomainFromResults(userSubmissions.Value.First());
            var table = $"|{userSubmissions.Key}|";
            var listResults = new List<LearningDomainOutcomeRecord>();
            
            foreach (var submission in userSubmissions.Value)
            {
                listResults.AddRange(submission.Results);
            }

            foreach (var rowTypes in domainFromResults!.RowsSet.Types)
            {
                table += $"|{rowTypes.Name}|";
            }
            table += "\n";


            foreach (var columnTypes in domainFromResults.ColumnsSet!.Types)
            {
                table += $"|{columnTypes.Name}|";
                foreach (var rowTypes in domainFromResults!.RowsSet.Types)
                {
                    var count = listResults.FindAll(r => r.Outcome.Column!.Id == columnTypes.Id && r.Outcome.Row.Id == rowTypes.Id).Count;
                    table += $"|{count}|";
                }
                table += "\n";
            }
            
            fileContents += table;
            

        }

        return fileContents;
    }
}