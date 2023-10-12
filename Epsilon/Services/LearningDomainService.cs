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

        if (domain != null)
        {
            if (domain.ColumnsSet != null)
            {
                domain.ColumnsSet.Types = domain.ColumnsSet.Types.OrderBy(static t => t.Order).ToList();
            }

            domain.RowsSet.Types = domain.RowsSet.Types.OrderBy(static t => t.Order).ToList();
            domain.ValuesSet.Types = domain.ValuesSet.Types.OrderBy(static t => t.Order).ToList();
        }
        
        return domain;
    }

    public async Task<IEnumerable<LearningDomainOutcome?>> GetOutcomes()
    {
        return await _learningDomainOutcomeRepository.AllToListAsync(includeProperties: s_learningDomainOutcomeProperties);
    }
}