using Microsoft.AspNetCore.Mvc.Testing;

namespace someapi2.Tests.Systems;

public class BasicTests
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
}