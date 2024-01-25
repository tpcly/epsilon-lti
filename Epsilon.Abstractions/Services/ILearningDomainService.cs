namespace Epsilon.Abstractions.Services;

public interface ILearningDomainService
{
    public Task<LearningDomain?> GetDomain(string name);

    public Task<IEnumerable<LearningDomainOutcome?>> GetOutcomes();

    public IEnumerable<LearningDomain?> GetDomainsFromTenant();
}