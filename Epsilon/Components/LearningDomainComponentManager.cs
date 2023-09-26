using Epsilon.Abstractions.Components;
using Epsilon.Abstractions.Services;

namespace Epsilon.Components;

public record LearningDomainComponentManager : IComponentManager
{
    private readonly ILearningDomainService _domainService;
    private readonly ILearningOutcomeCanvasResultService _canvasResultService;
    
    public LearningDomainComponentManager(ILearningDomainService domainService, ILearningOutcomeCanvasResultService canvasResultService)
    {
        _domainService = domainService;
        _canvasResultService = canvasResultService;
    }
    
    public async Task<IWordCompetenceComponent> Fetch(int courseId, string pageName, string userId, string domainName)
    {
        var domain = await _domainService.GetDomain(domainName);
        var domainOutcome = await _domainService.GetOutcomes();
        var submissions =  _canvasResultService.GetSubmissions(userId);
        return new LearningDomainComponent(domainOutcome, domain, submissions);
    }
}