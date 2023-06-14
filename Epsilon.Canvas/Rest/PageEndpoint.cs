using System.Net;
using System.Net.Http.Json;
using Epsilon.Canvas.Abstractions.Rest;

namespace Epsilon.Canvas.Rest;

public class PageEndpoint : IPageEndpoint
{
    private readonly HttpClient _client;

    public PageEndpoint(HttpClient client)
    {
        _client = client;
    }

    public async Task<Page?> GetPage(int courseId, string id)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages/{id}");
        using var response = await _client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<Page>()
            : null;
    }
    
    public async Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages");
        using var response = await _client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<IEnumerable<Page>>()
            : null;
    }
    
    public async Task<Page?> CreatePage(int courseId, Page page)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/courses/{courseId}/pages")
        {
            Content = JsonContent.Create( new { wiki_page = new { title = page.Url, body = page.Body, },}),
        };

        using var response = await _client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<Page?>()
            : null;
    }
    
    public async Task<Page?> UpdatePage(int courseId, Page page)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"v1/courses/{courseId}/pages/{page.Url}")
        {
            Content = JsonContent.Create( new { wiki_page = new { title = page.Url, body = page.Body, },}),
        };

        using var response = await _client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<Page?>()
            : null;
    }
    
    public async Task<Page?> UpdateOrCreatePage(int courseId, Page page)
    {
        if (page.PageId == null && page.Url == null)
        {
            throw new ArgumentNullException(nameof(page));
        }

        var id = page.PageId != null
            ? page.PageId.ToString()
            : page.Url;
        
        var existingPage = await GetPage(courseId, id!);

        if (existingPage == null)
        {
            return await CreatePage(courseId, page);
        }

        return await UpdatePage(courseId, page);
    }
    
    public async Task<IEnumerable<PageRevision>?> GetRevisions(int courseId, string pageId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages/{pageId}/revisions");
        using var response = await _client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<IEnumerable<PageRevision>>()
            : null;
    }

    public async Task<PageRevision?> GetClosestRevisionByDates(int courseId, string pageId, DateTime startDate, DateTime endDate)
    {
        var revisions = await GetRevisions(courseId, pageId);
        var closestRevision = revisions!
                              .OrderBy(obj => Math.Abs((obj.UpdatedAt!.Value - startDate).Ticks))
                              .FirstOrDefault(obj => obj.UpdatedAt >= startDate && obj.UpdatedAt <= endDate);

        return closestRevision;
    }

    public async Task<PageRevision?> GetRevision(int courseId, string pageId, int revisionId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages/{pageId}/revisions/{revisionId}");
        using var response = await _client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<PageRevision>()
            : null;
    }
}