using Microsoft.AspNetCore.Mvc.Testing;

namespace someapi2.Tests.Systems;

public class BasicTests
{
    [Fact]
    public async Task GetTodos()
    {
        var factory = new WebApplicationFactory<someapi2.IAssemblyMarker>();
        var client = factory.CreateClient();
        var response = await client.GetAsync("/todos");
        response.EnsureSuccessStatusCode();
    }
}