using Businesses.API.IntegrationTests.API.MockModels;
using System.Net.Http.Headers;

namespace Businesses.API.IntegrationTests.API.Extensions;

public static class HttpClientExtensions
{
    public static HttpClient WithJwtBearerToken(this HttpClient client, Action<TestJwtToken> configure)
    {
        var token = new TestJwtToken();
        configure(token);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Build());
        return client;
    }
}
