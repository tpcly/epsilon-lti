namespace Epsilon.Abstractions.Components;

public interface ICompetenceComponentFetcher
{
    public Task<ICompetenceComponent> FetchUnknown(DateTime startDate, DateTime endDate);
}