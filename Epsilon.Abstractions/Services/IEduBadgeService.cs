using System.Collections.ObjectModel;
namespace Epsilon.Abstractions.Services;

public interface IEduBadgeService
{
    Task<Dictionary<string, List<LearningDomainSubmission>>> GetData(Collection<string> userIds, DateTime from, DateTime to);

    public Task<string> WriteDocument(Dictionary<string, List<LearningDomainSubmission>> data);
}