namespace Epsilon.Canvas.Abstractions.GraphQl;

public interface ICanvasGraphQlApi
{
    public Task<T?> Query<T>(string query);
}