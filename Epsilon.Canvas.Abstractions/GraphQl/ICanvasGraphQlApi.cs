namespace Epsilon.Canvas.Abstractions.GraphQl;

public interface ICanvasGraphQlApi
{
    public Task<GraphQlSchema?> Query(string query, IDictionary<string, object>? variables = null);
}