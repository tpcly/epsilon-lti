namespace Epsilon.Abstractions.Services;

public interface ILearningDomainService
{
    public Task<LearningDomain?> GetDomain(string name);
}