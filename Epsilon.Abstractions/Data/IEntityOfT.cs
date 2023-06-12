namespace Epsilon.Abstractions.Data;

public interface IEntity<TKey> : IEntity
{
    public new TKey? Id { get; set; }
}