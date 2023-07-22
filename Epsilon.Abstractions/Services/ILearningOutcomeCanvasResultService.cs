namespace Epsilon.Abstractions.Services;

public interface ILearningOutcomeCanvasResultService
{
    IAsyncEnumerable<LearningDomainOutcomeResult> GetOutcomeResults(string studentId, DateTime startDate, DateTime endDate);
}