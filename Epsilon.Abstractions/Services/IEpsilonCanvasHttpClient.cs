namespace Epsilon.Abstractions.Services;

public interface IEpsilonCanvasHttpClient
{

    public Task<HttpResponseMessage> Request(HttpMethod method, string path, HttpContent? body = null);
}