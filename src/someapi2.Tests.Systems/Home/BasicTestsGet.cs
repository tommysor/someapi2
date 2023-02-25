using System.Net;

namespace someapi2.Tests.Systems.Home;

public sealed class BasicTestsGet
{
    private readonly HttpClient _httpClient;

    public BasicTestsGet()
    {
        _httpClient = TestWebApplicationFactory.CreateClientAnonymous();
    }

    [Fact]
    public async Task GetResponds()
    {
        var response = await _httpClient.GetAsync("/");
        response.EnsureSuccessStatusCode();
    }

   [Fact]
    public async Task IsRateLimited()
    {
        const int maxRequests = 3;
        for (var i = 0; i < maxRequests; i++)
        {
            var okResponse = await _httpClient.GetAsync("/");
            okResponse.EnsureSuccessStatusCode();
        }

        var response = await _httpClient.GetAsync("/");
        Assert.Equal(HttpStatusCode.TooManyRequests, response.StatusCode);
    }

    [Fact]
    public async Task AllowsAnonymousUser()
    {
        var client = TestWebApplicationFactory.CreateClientAnonymous();
        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();
    }
}
