using System.Diagnostics.CodeAnalysis;

namespace Epsilon.Canvas.Abstractions.Rest;

public interface IPageEndpoint
{
    Task<Page?> GetPage(int courseId, string id);

    Task<IEnumerable<Page>?> GetAll(int courseId);

    //Suppressed for later implementation. Remove when implemented  
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    Task<Page?> CreatePage(int courseId, Page page);

    //Suppressed for later implementation. Remove when implemented  
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    Task<Page?> UpdatePage(int courseId, Page page);

    Task<Page?> UpdateOrCreatePage(int courseId, Page page);

    //Suppressed for later implementation. Remove when implemented  
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    Task<IEnumerable<PageRevision>?> GetAllRevisions(int courseId, string pageId);

    Task<PageRevision?> GetRevision(int courseId, string pageId, int revisionId);

    Task<PageRevision?> GetClosestRevisionByDates(int courseId, string pageId, DateTime startDate, DateTime endDate);
}