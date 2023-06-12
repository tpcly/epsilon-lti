namespace Epsilon.Canvas.Abstractions.Rest;

public interface IPageEndpoint
{
    Task<IEnumerable<PageRevision>?> GetRevisions(int courseId, string pageId);

    Task<PageRevision?> GetRevision(int courseId, string pageId, int revisionId);
    
    Task<PageRevision?> GetClosestRevisionByDates(int courseId, string pageId, DateTime startDate, DateTime endDate);

    Task<Page?> UpdateOrCreatePage(int courseId, string id, string html);

    Task<Page?> CreatePage(int courseId, string id, string html);

    Task<Page?> UpdatePage(int courseId, string id, string html);
    
    Task<Page?> GetPage(int courseId, string id);

    Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include);
}