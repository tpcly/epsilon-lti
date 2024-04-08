namespace Epsilon.Abstractions.Services;

public interface ILearningOutcomeCanvasResultService
{
    public IAsyncEnumerable<LearningDomainSubmission> GetSubmissions(string studentId,  DateTime? gradedSince = null);
}