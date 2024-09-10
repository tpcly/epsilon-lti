using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;

namespace Epsilon.Services;

public class EduBadgeService : IEduBadgeService
{
    private readonly ILearningOutcomeCanvasResultService _canvasResultService;

    public EduBadgeService(ILearningOutcomeCanvasResultService canvasResultService)
    {
        _canvasResultService = canvasResultService;
    }

    public async Task<List<LearningDomainSubmission>> GetData(ICollection<string> userIds, DateTime from, DateTime to)
    {
        return await _canvasResultService.GetSubmissions(userIds.First(), from)
                                                         .Where(s => s.SubmittedAt <= to && s.Criteria.Any())
                                                         .ToListAsync();
        
    }

    public void WriteDocument(Stream stream, IEnumerable<IAsyncEnumerable<LearningDomainSubmission>> data)
    {
        throw new NotImplementedException();
    }
}