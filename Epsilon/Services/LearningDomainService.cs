using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Tpcly.Persistence.Abstractions;

namespace Epsilon.Services;

public class LearningDomainService : ILearningDomainService
{
    private static readonly string[] s_learningDomainProperties = { "RowsSet", "RowsSet.Types", "ColumnsSet", "ColumnsSet.Types", "ValuesSet", "ValuesSet.Types", };
    private static readonly string[] s_learningDomainOutcomeProperties = { "Row", "Column", "Value", };

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
        var domain = await _learningDomainRepository.SingleOrDefaultAsync(d => d.Id == name, includeProperties: s_learningDomainProperties);
        
        domain?.Order();
        
        return domain;
    }

    public async Task<IEnumerable<LearningDomain?>> GetDomainsFromTenant()
    {
        var domains = await _learningDomainRepository.AllToListAsync(includeProperties: s_learningDomainProperties);
        return domains;

    }

    public async Task<IEnumerable<LearningDomainOutcome?>> GetOutcomes()
    {
        return await _learningDomainOutcomeRepository.AllToListAsync(includeProperties: s_learningDomainOutcomeProperties);
    }
}