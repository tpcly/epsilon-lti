using System.Net.Http.Json;
using Epsilon.Canvas.Abstractions.GraphQl;

namespace Epsilon.Canvas.GraphQl;

public class CanvasGraphQlApi : ICanvasGraphQlApi
{
    private readonly HttpClient _client;

    public CanvasGraphQlApi(HttpClient client)
    {
        _client = client;
    }

    public async Task<T?> Query<T>(string query)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/graphql")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {
                    "query", query
                },
            }),
        };

        var response = await _client.SendAsync(request);

        return await response.Content.ReadFromJsonAsync<T>();
    }
}