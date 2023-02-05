using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace someapi2.Tests.Systems;

public sealed class BasicTests
{
    private readonly HttpClient _httpClient;

    public BasicTests()
    {
        var factory = new WebApplicationFactory<someapi2.IAssemblyMarker>();
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetTodosResponds()
    {
        var response = await _httpClient.GetAsync("/todos");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetTodosIsRateLimited()
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
    public async Task GetTodosIsRateLimitedOpensUpAgain()
    {
        const int maxRequests = 3;
        for (var i = 0; i < maxRequests; i++)
        {
            var okResponse = await _httpClient.GetAsync("/todos");
            okResponse.EnsureSuccessStatusCode();
        }

        var response = await _httpClient.GetAsync("/todos");
        Assert.Equal(HttpStatusCode.TooManyRequests, response.StatusCode);

        await Task.Delay(TimeSpan.FromSeconds(1.5));

        var okResponse2 = await _httpClient.GetAsync("/todos");
        okResponse2.EnsureSuccessStatusCode();
    }
}
