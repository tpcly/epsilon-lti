using System.Collections.ObjectModel;
using System.Globalization;
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
            var result = await _canvasResultService.GetSubmissions(user.Id.ToString(CultureInfo.CurrentCulture), from)
                                                   .Where(e => e.Criteria.Any() && e.SubmittedAt <= to)
                                                   .ToListAsync();

            if (result.Any(static r => r.Results.Any(static rr => rr.Grade >= 3)))
                results.Add(user.Id.ToString(CultureInfo.CurrentCulture), result);
        }

        return results;
    }

    // private static string CreateHorizontalLine(int columns)
    // {
    //     var line = "|";
    //     for (var i = 0; i < columns; i++)
    //         line += "---|";
    //
    //     return line;
    // }

    private async Task<string> GenerateTableDelta(KeyValuePair<string, List<LearningDomainSubmission>> userSubmissions)
    {
        var domainFromResults = await _learningDomainService.GetDomain("hbo-i-2018");
        
        var listResults = new List<LearningDomainOutcomeRecord>();

        foreach (var submission in userSubmissions.Value)
            listResults.AddRange(submission.Results);

        var lengthFirstColumn = domainFromResults!.RowsSet!.Types.Max(static c => c.Name.Length);
        var table = $"| {FillColumn(lengthFirstColumn)} ";
        foreach (var rowTypes in domainFromResults!.ColumnsSet!.Types.OrderBy(static r => r.Order))
            table += $"| {rowTypes.Name} ";

        table += "|";


        foreach (var rowTypes in domainFromResults.RowsSet.Types.OrderBy(static r => r.Order))
        {
            table += $"| {rowTypes.Name} {FillColumn(lengthFirstColumn - rowTypes.Name.Length)}";
            
            foreach (var columnTypes in domainFromResults!.ColumnsSet.Types.OrderBy(static r => r.Order))
            {
                var lengthColumn = columnTypes.Name.Length; 
                var count = listResults.Count(r => r.Outcome.Column?.Id == columnTypes.Id && r.Outcome.Row.Id == rowTypes.Id && r.Grade >= 3);
                table += $"| {count} {FillColumn(lengthColumn - count.ToString(CultureInfo.InvariantCulture).Length)}";
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
            var markdownTable = await GenerateTableDelta(userSubmissions);
#pragma warning disable CA1307
            var escapedMarkdownTable = markdownTable.Replace("||", "|<br/>|");
#pragma warning restore CA1307
#pragma warning disable CA1305
            csvBuilder.AppendLine($"{userSubmissions.Key},\"{escapedMarkdownTable}\"");
        }

        return csvBuilder.ToString();
    }
    
    
    private static string FillColumn(int length)
    {
        if (length < 0)
            length = 0;
        return new string(' ', length);
    }
}