using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace someapi2.Tests.Systems;

public sealed class BasicHomeTests
{
    private readonly HttpClient _httpClient;

    public BasicHomeTests()
    {
        var factory = new WebApplicationFactory<someapi2.IAssemblyMarker>();
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetHomeResponds()
    {
        var response = await _httpClient.GetAsync("/");
        response.EnsureSuccessStatusCode();
    }

   [Fact]
    public async Task GetHomeIsRateLimited()
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
    public async Task GetHomeIsRateLimitedOpensUpAgain()
    {
        const int maxRequests = 3;
        for (var i = 0; i < maxRequests; i++)
        {
            var okResponse = await _httpClient.GetAsync("/");
            okResponse.EnsureSuccessStatusCode();
        }

        var response = await _httpClient.GetAsync("/");
        Assert.Equal(HttpStatusCode.TooManyRequests, response.StatusCode);

        await Task.Delay(TimeSpan.FromSeconds(1.5));

        var okResponse2 = await _httpClient.GetAsync("/");
        okResponse2.EnsureSuccessStatusCode();
    }
}
