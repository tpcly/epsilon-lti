namespace Epsilon.Abstractions;

public interface ICanvasUserSessionAccessor
{
    public Task<CanvasUserSession?> GetSessionAsync();
}