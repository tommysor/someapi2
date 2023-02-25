using System.Net;

namespace someapi2.Tests.Systems.Todos;

public sealed class BasicTestsGetById
{
    private readonly HttpClient _httpClient;

    public BasicTestsGetById()
    {
        _httpClient = TestWebApplicationFactory.CreateClientAnonymous();
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
