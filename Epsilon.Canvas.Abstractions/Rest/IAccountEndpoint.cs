namespace Epsilon.Canvas.Abstractions.Rest;

public interface IAccountEndpoint
{
    public Task<IEnumerable<EnrollmentTerm>?> GetTerms(int accountId, int limit = 100);
}