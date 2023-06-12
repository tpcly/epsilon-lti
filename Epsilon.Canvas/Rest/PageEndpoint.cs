using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Epsilon.Canvas.Abstractions.Rest;

namespace Epsilon.Canvas.Rest;

public class PageEndpoint : IPageEndpoint
{
    private readonly HttpClient _client;
    
    public PageEndpoint(HttpClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<Revision>?> GetRevisions(int courseId, string id)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages/{id}/revisions");
        using var response = await _client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<IEnumerable<Revision>>()
            : null;
    }
    
    public async Task<Page?> UpdateOrCreatePage(int courseId, string id, string html)
    {
        var existingPage = await GetPage(courseId, id);

        if (existingPage == null)
        {
            return await CreatePage(courseId, id, html);
        }

        return await UpdatePage(courseId, id, html);
    }
    
    public async Task<Page?> CreatePage(int courseId, string id, string html)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/courses/{courseId}/pages");
        var pageCreation = new { wiki_page = new { title = id, body = html, }, };
        request.Content = new StringContent(JsonSerializer.Serialize(pageCreation), Encoding.UTF8, "application/json");

        using var response = await _client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<Page?>()
            : null;
    }

    public async Task<Page?> UpdatePage(int courseId, string id, string html)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"v1/courses/{courseId}/pages/{id}");
        var pageUpdate = new { wiki_page = new { title = id, body = html, }, };
        request.Content = new StringContent(JsonSerializer.Serialize(pageUpdate), Encoding.UTF8, "application/json");

        using var response = await _client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<Page?>()
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

    public async Task<Page?> GetPage(int courseId, string id)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages/{id}");
        using var response = await _client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<Page>()
            : null;
    }
}