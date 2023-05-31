namespace Epsilon.Canvas.Abstractions.Rest;

public interface IFileEndpoint
{
    Task<IEnumerable<byte>?> GetByteArray(Uri url);
}