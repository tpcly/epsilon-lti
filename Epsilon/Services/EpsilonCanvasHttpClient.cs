using Epsilon.Abstractions.Services;
namespace Epsilon.Services;

public class EpsilonCanvasHttpClient : IEpsilonCanvasHttpClient
{
    private readonly HttpClient _client;
    
    
    public EpsilonCanvasHttpClient(HttpClient client)
    {
        _client = client;
    }
    
    
    public async Task<HttpResponseMessage> Request(HttpMethod method, string path, HttpContent? body)
    {
        using var request = new HttpRequestMessage(method, path);
        request.Content = body;
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return response;
    }
}