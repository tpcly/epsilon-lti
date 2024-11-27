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

    public async Task<Dictionary<string, List<LearningDomainSubmission>>> GetData(Collection<string> searchQuery, DateTime from, DateTime to)
    {
        var results = new Dictionary<string, List<LearningDomainSubmission>>();
        foreach (var search in searchQuery)

        {
            var users = await _canvasResultService.SearchUsers(1, search);
            var user = users?.First() ?? throw new HttpRequestException("No user found");
            var result = await _canvasResultService.GetSubmissions(user.Id, from)
                                                   .Where(e => e.Criteria.Any() && e.SubmittedAt <= to)
                                                   .ToListAsync();

            if (result.Any(static r => r.Results.Any(static rr => rr.Grade >= 3)))
                results.Add(user.Id, result);
        }

        return results;
    }

    private static string CreateHorizontalLine(int columns)
    {
        var line = "|";
        for (var i = 0; i < columns; i++)
            line += "---|";

        return line;
    }

    private async Task<string> GenerateMarkdownDelta(KeyValuePair<string, List<LearningDomainSubmission>> userSubmissions)
    {
        var domainFromResults = await _learningDomainService.GetDomain("hbo-i-2018");
        var table = $"|{userSubmissions.Key}";
        var listResults = new List<LearningDomainOutcomeRecord>();

        foreach (var submission in userSubmissions.Value)
            listResults.AddRange(submission.Results);

        foreach (var rowTypes in domainFromResults!.ColumnsSet!.Types.OrderBy(static r => r.Order))
            table += $"|{rowTypes.Name}";

        table += "|";
        table += CreateHorizontalLine(domainFromResults.ColumnsSet!.Types.Count() + 1);
        table += "";


        foreach (var rowTypes in domainFromResults.RowsSet.Types.OrderBy(static r => r.Order))
        {
            table += $"|{rowTypes.Name}";
            foreach (var columnTypes in domainFromResults!.ColumnsSet.Types.OrderBy(static r => r.Order))
            {
                var count = listResults.Count(r => r.Outcome.Column?.Id == columnTypes.Id && r.Outcome.Row.Id == rowTypes.Id && r.Grade >= 3);
                table += $"|{count}";
            }

            table += "|";
        }

        return table;
    }

    public async Task<string> WriteDocument(Dictionary<string, List<LearningDomainSubmission>> data)
    {
        var csvBuilder = new StringBuilder();

        csvBuilder.AppendLine("Student-Id,Delta-Table");
        foreach (var userSubmissions in data)
        {
            var markdownTable = await GenerateMarkdownDelta(userSubmissions);
#pragma warning disable CA1307
            var escapedMarkdownTable = markdownTable.Replace("||", "|\n|");
#pragma warning restore CA1307
#pragma warning disable CA1305
            csvBuilder.AppendLine($"{userSubmissions.Key},\"{escapedMarkdownTable}\"");
        }
#pragma warning restore CA1305

        return csvBuilder.ToString();
    }
}