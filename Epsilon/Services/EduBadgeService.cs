using System.Collections.ObjectModel;
using System.Text;
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
            var result = await _canvasResultService.GetSubmissions(userId, from)
                                                   .Where(static e => e.Criteria.Any())
                                                   .ToListAsync();
            results.Add(userId, result);
        }

        return results;
    }

    private static string CreateHorizontalLine(int columns)
    {
        var line = "|";
        for (var i = 0; i < columns; i++)
        {
            line += "---|";
        }

        return line;
    }

    private async Task<string> GenerateMarkdownDelta(KeyValuePair<string, List<LearningDomainSubmission>> userSubmissions)
    {
        var domainFromResults = await _learningDomainService.GetDomain("hbo-i-2018");
        var table = $"|{userSubmissions.Key}";
        var listResults = new List<LearningDomainOutcomeRecord>();

        foreach (var submission in userSubmissions.Value)
        {
            listResults.AddRange(submission.Results);
        }

        foreach (var rowTypes in domainFromResults!.RowsSet.Types)
        {
            table += $"|{rowTypes.Name}";
        }

        table += "|\n";
        table += CreateHorizontalLine(domainFromResults.RowsSet.Types.Count() + 1);
        table += "\n";


        foreach (var columnTypes in domainFromResults.ColumnsSet!.Types)
        {
            table += $"|{columnTypes.Name}";
            foreach (var rowTypes in domainFromResults!.RowsSet.Types)
            {
                var count = listResults.Count(r => r.Outcome.Column?.Id == columnTypes.Id && r.Outcome.Row.Id == rowTypes.Id);
                table += $"|{count}";
            }

            table += "|\n";
        }

        return table;
    }

    public async Task<string> WriteDocument(Dictionary<string, List<LearningDomainSubmission>> data)
    {
        var csvBuilder = new StringBuilder();
        
        csvBuilder.AppendLine("Key,Value");
        foreach (var userSubmissions in data)
        {
            csvBuilder.AppendLine(userSubmissions.Key + "," + await GenerateMarkdownDelta(userSubmissions));
        }

        return csvBuilder.ToString();
    }
}