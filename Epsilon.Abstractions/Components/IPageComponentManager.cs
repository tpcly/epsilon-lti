namespace Epsilon.Abstractions.Components;

public interface IPageComponentManager
{
    public Task<PageComponent> Fetch(int courseId, string pageName);
    public Task<PageComponent> CreateOrUpdate(int courseId, string pageName, string body);
}