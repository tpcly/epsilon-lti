using Epsilon.Abstractions;
using Epsilon.Abstractions.Data;
using Epsilon.Abstractions.Services;

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
        return await _learningDomainRepository.SingleOrDefaultAsync(d => d.Id == name, includeProperties: s_learningDomainProperties);
    }

    public async Task<IEnumerable<LearningDomainOutcome?>> GetOutcomes()
    {
        return await _learningDomainOutcomeRepository.AllToListAsync(includeProperties: s_learningDomainOutcomeProperties);
    }
}