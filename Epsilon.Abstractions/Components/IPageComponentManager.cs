namespace Epsilon.Abstractions.Components;

public interface IPageComponentManager: IComponentManager
{
    public Task<PageComponent> CreateOrUpdate(int courseId, string pageName, string body);
}