namespace Epsilon.Canvas.Abstractions.Rest;

public interface IPageEndpoint
{
    Task<Page?> GetPageByName(int courseId, string pageName);

    Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include);
}