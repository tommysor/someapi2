using System.Net;

namespace someapi2.Tests.Systems.Todos;

public sealed class BasicTestsGet
{
    private readonly HttpClient _httpClient;

    public BasicTestsGet()
    {
        _httpClient = TestWebApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task GetResponds()
    {
        var response = await _httpClient.GetAsync("/todos");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task IsRateLimited()
    {
        const int maxRequests = 3;
        for (var i = 0; i < maxRequests; i++)
        {
            var okResponse = await _httpClient.GetAsync("/todos");
            okResponse.EnsureSuccessStatusCode();
        }

        var response = await _httpClient.GetAsync("/todos");
        Assert.Equal(HttpStatusCode.TooManyRequests, response.StatusCode);
    }

    [Fact]
    public async Task RequiresAuthenticatedUser()
    {
        var client = TestWebApplicationFactory.CreateClientAnonymous();
        var response = await client.GetAsync("/todos");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
