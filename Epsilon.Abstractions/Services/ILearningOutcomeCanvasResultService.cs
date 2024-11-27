using User = Tpcly.Canvas.Abstractions.Rest.User;
namespace Epsilon.Abstractions.Services;

public interface ILearningOutcomeCanvasResultService
{
    public IAsyncEnumerable<LearningDomainSubmission> GetSubmissions(string studentId,  DateTime? submittedSince = null);

    public Task<IEnumerable<User>?> SearchUsers(int accountId, string query);
}