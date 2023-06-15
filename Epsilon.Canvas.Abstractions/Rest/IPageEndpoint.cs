namespace Epsilon.Canvas.Abstractions.Rest;

public interface IPageEndpoint
{
    Task<Page?> GetPage(int courseId, string id);
    
    Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include);

    Task<Page?> CreatePage(int courseId, Page page);

    Task<Page?> UpdatePage(int courseId, Page page);
    
    Task<Page?> UpdateOrCreatePage(int courseId, Page page);

    Task<IEnumerable<PageRevision>?> GetAllRevisions(int courseId, string pageId);

    Task<PageRevision?> GetRevision(int courseId, string pageId, int revisionId);
    
    Task<PageRevision?> GetClosestRevisionByDates(int courseId, string pageId, DateTime startDate, DateTime endDate);
}