using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Tpcly.Persistence.Abstractions;

namespace Epsilon.Services;

public class LearningDomainService : ILearningDomainService
{
    private static readonly string[] s_learningDomainProperties = { "RowsSet", "RowsSet.Types", "ColumnsSet", "ColumnsSet.Types", "ValuesSet", "ValuesSet.Types", };
    private static readonly string[] s_learningDomainOutcomeProperties = { "Row", "Column", "Value", "Domain", };

    private readonly IReadOnlyRepository<LearningDomain> _learningDomainRepository;
    private readonly IReadOnlyRepository<LearningDomainOutcome> _learningDomainOutcomeRepository;

    public LearningDomainService(
        IReadOnlyRepository<LearningDomain> learningDomainRepository,
        IReadOnlyRepository<LearningDomainOutcome> learningDomainOutcomeRepository
    )
    {
        _learningDomainRepository = learningDomainRepository;
        _learningDomainOutcomeRepository = learningDomainOutcomeRepository;
    }

    public async Task<LearningDomain?> GetDomain(string name)
    {
        var domain = await _learningDomainRepository.FirstOrDefaultAsync(
            d => d.Id == name,
            includeProperties: s_learningDomainProperties);

        return domain;
    }

    public async Task<LearningDomain?> GetDomainFromResults(IEnumerable<LearningDomainSubmission> submissions)
    {
        // var learningDomainSubmissions = submissions.ToList();
        // if (learningDomainSubmissions.Count != 0)
        // {
        //     var results = learningDomainSubmissions.Select(static s => s.Results.First(static r => r.Outcome.Column != null));
        //     // var domainId = (await _learningDomainOutcomeRepository.FindAsync(results.Where(static r => r.Outcome.Column != null).First()))!.Domain.Id;
        //     return await GetDomain("hbo-i-2018");
        // }

        return await GetDomain("hbo-i-2018");
    }

    public IEnumerable<LearningDomain?> GetDomainsFromTenant()
    {
        return _learningDomainRepository.AllToList(includeProperties: s_learningDomainProperties);
    }

    public async Task<IEnumerable<LearningDomainOutcome?>> GetOutcomes()
    {
        return await _learningDomainOutcomeRepository.AllToListAsync(includeProperties: s_learningDomainOutcomeProperties);
    }
}