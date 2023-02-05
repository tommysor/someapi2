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

    [Fact]
    public async Task GetTodosByIdResponds()
    {
        var response = await _httpClient.GetAsync("/todos/10000000-0000-0000-0000-000000000000");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetTodosByIdEmptyGuidReturnsBadRequest()
    {
        var emptyGuid = Guid.Empty.ToString();
        var response = await _httpClient.GetAsync($"/todos/{emptyGuid}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTodosByIdEmptyGuidWithoutDashReturnsBadRequest()
    {
        var emptyGuid = Guid.Empty.ToString("N");
        var response = await _httpClient.GetAsync($"/todos/{emptyGuid}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task GetTodosByIdIntReturnsBadRequest()
    {
        var response = await _httpClient.GetAsync("/todos/1");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTodosByIdStringReturnsBadRequest()
    {
        var response = await _httpClient.GetAsync("/todos/abc");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
