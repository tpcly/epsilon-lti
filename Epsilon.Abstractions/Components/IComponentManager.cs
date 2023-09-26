namespace Epsilon.Abstractions.Components;

public interface IComponentManager
{
    public Task<IWordCompetenceComponent> Fetch(int courseId, string pageName, string userId, string domainName);
}