using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using CsvHelper;
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
            var result = await _canvasResultService.GetSubmissions(user.Id.ToString(CultureInfo.CurrentCulture), from)
                                                   .Where(e => e.Criteria.Any() && e.SubmittedAt <= to)
                                                   .ToListAsync();

            if (result.Any(static r => r.Results.Any(static rr => rr.Grade >= 3)))
                results.Add(user.LoginId, result);
        }

        return results;
    }

    private async Task<string> GenerateTableDelta(KeyValuePair<string, List<LearningDomainSubmission>> userSubmissions)
    {
        var domainFromResults = await _learningDomainService.GetDomain("hbo-i-2018");
        
        var listResults = new List<LearningDomainOutcomeRecord>();

        foreach (var submission in userSubmissions.Value)
            listResults.AddRange(submission.Results);
        
        var table = "<table><tr><td></td>";
        foreach (var rowTypes in domainFromResults!.ColumnsSet!.Types.OrderBy(static r => r.Order))
            table += $"<th>{rowTypes.Name.Replace('&', ' ')}</th>";

        table += "</tr>";


        foreach (var rowTypes in domainFromResults.RowsSet.Types.OrderBy(static r => r.Order))
        {
            table += $"<tr><th>{rowTypes.Name.Replace('&', ' ')}</th>";
            
            foreach (var columnTypes in domainFromResults!.ColumnsSet.Types.OrderBy(static r => r.Order))
            {
                var count = listResults.Count(r => r.Outcome.Column?.Id == columnTypes.Id && r.Outcome.Row.Id == rowTypes.Id && r.Grade >= 3);
                table += $"<td>{count}</td>";
            }

            table += "</tr>";
        }

        table += "</table>";

        return table;
    }
    
    private static string CollectAssignmentUrls(List<LearningDomainSubmission> submissions)
    {
        var urls = new StringBuilder();
        foreach (var submission in submissions)
        {
            if (submission.AssignmentUrl != null)
                urls.Append(submission.AssignmentUrl).Append(' ');
        }

        return urls.ToString();
    }

    public async Task<string> WriteDocument(Dictionary<string, List<LearningDomainSubmission>> data)
    {
        var collection = new List<EdubadgeRecord>();
        foreach (var userSubmissions in data)
        {
            collection.Add(new EdubadgeRecord()
            {
                Email = userSubmissions.Key,
                Eppn = userSubmissions.Key,
                Narrative = $"{await GenerateTableDelta(userSubmissions)}",
                EvidenceDescription = $"{CollectAssignmentUrls(userSubmissions.Value)}", 
             });
        }

        await using var writer = new StringWriter();
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(collection);
        return writer.ToString();
    }
    
}