namespace Epsilon.Abstractions.Components;

public interface ICompetenceComponentFetcher<TComponent> : ICompetenceComponentFetcher
    where TComponent : ICompetenceComponent
{
    public Task<TComponent> Fetch(DateTime startDate, DateTime endDate);
}