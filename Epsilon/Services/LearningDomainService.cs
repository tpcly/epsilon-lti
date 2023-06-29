using Epsilon.Abstractions;
using Epsilon.Abstractions.Data;
using Epsilon.Abstractions.Services;

namespace Epsilon.Services;

public class LearningDomainService : ILearningDomainService
{
    private static readonly string[] s_learningDomainProperties = { "RowsSet", "RowsSet.Types", "ColumnsSet", "ColumnsSet.Types", "ValuesSet", "ValuesSet.Types", };

    private readonly IReadOnlyRepository<LearningDomain> _learningDomainRepository;

    public LearningDomainService(IReadOnlyRepository<LearningDomain> learningDomainRepository)
    {
        _learningDomainRepository = learningDomainRepository;
    }

    public async Task<LearningDomain?> GetDomain(string name)
    {
        return await _learningDomainRepository.SingleOrDefaultAsync(d => d.Id == name, includeProperties: s_learningDomainProperties);
    }
}