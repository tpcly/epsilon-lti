using System.Collections.ObjectModel;
namespace Epsilon.Abstractions.Services;

public interface IEduBadgeService
{
    Task<List<LearningDomainSubmission>> GetData(Collection<string> userIds, DateTime from, DateTime to);

    void WriteDocument(Stream stream, IEnumerable<IAsyncEnumerable<LearningDomainSubmission>> data);
}