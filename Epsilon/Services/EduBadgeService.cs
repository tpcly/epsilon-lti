using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;

namespace Epsilon.Services;

public class EduBadgeService : IEduBadgeService
{
    private readonly LearningOutcomeCanvasResultService _canvasResultService;

    public EduBadgeService(LearningOutcomeCanvasResultService canvasResultService)
    {
        _canvasResultService = canvasResultService;
    }

    public IEnumerable<IAsyncEnumerable<LearningDomainSubmission>> GetData(IEnumerable<string> userIds, DateTime from, DateTime to)
    {
        foreach (var userId in userIds)
        {
            yield return _canvasResultService.GetSubmissions(userId);
        }
    }

    public void WriteDocument(Stream stream, IEnumerable<IAsyncEnumerable<LearningDomainSubmission>> data)
    {
        throw new NotImplementedException();
    }
}