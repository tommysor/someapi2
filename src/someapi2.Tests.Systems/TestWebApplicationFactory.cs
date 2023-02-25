using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Options;

namespace someapi2.Tests.Systems;

public static class TestWebApplicationFactory
{
    private static WebApplicationFactory<IAssemblyMarker> GetFactory()
    {
        var factory = new WebApplicationFactory<IAssemblyMarker>()
            .WithWebHostBuilder(host =>
            {
                host.ConfigureTestServices(services =>
                {
                    
                });

            });

        return factory;
    }

    public static HttpClient CreateClientAnonymous()
    {
        var factory = GetFactory();
        var client = factory.CreateClient();
        return client;
    }
}
