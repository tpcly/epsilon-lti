namespace Epsilon.Canvas.Abstractions.Rest;

public interface IPageEndpoint
{
    Task<IEnumerable<Revision>?> GetRevisions(int courseId, string id);

    // Task<Revision?> GetRevision(int courseId, string id, DateTime startDate, DateTime endDate);
    
    Task<Page?> UpdateOrCreatePage(int courseId, string id, string html);

    Task<Page?> CreatePage(int courseId, string id, string html);

    Task<Page?> UpdatePage(int courseId, string id, string html);
    
    Task<Page?> GetPage(int courseId, string id);

    Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include);
}