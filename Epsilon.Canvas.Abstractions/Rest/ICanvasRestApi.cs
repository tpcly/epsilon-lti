namespace Epsilon.Canvas.Abstractions.Rest;

public interface ICanvasRestApi
{
    public IAccountEndpoint Accounts { get; }

    public IFileEndpoint Files { get; }

    public IPageEndpoint Pages { get; }
}