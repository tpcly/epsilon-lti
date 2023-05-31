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

    public async Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages");
        using var response = await _client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<IEnumerable<Page>>()
            : null;
    }

    public async Task<Page?> GetPageByName(int courseId, string pageName)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages/{pageName}");
        using var response = await _client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<Page>()
            : null;
    }
}