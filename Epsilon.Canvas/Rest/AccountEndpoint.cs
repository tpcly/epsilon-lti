using System.Net.Http.Json;
using Epsilon.Canvas.Abstractions.Rest;

namespace Epsilon.Canvas.Rest;

public class AccountEndpoint : IAccountEndpoint
{
    private readonly HttpClient _client;

    public AccountEndpoint(HttpClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<EnrollmentTerm>?> GetTerms(int accountId, int limit = 100)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/accounts/{accountId}/terms?per_page={limit}");
        var response = await _client.SendAsync(request);
        var collection = await response.Content.ReadFromJsonAsync<EnrollmentTermCollection>();

        return collection?.Terms;
    }
}