namespace Epsilon.Abstractions.Components;

public interface IFetcher<TComponent>
{
    public Task<TComponent> Fetch(DateTime from, DateTime to);
}