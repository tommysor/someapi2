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
                    services
                        .AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, AuthenticationTestHandler>("Test", null);
                    services
                        .AddAuthorization(options =>
                        {
                            var policy = new AuthorizationPolicyBuilder()
                                .AddAuthenticationSchemes("Test")
                                .RequireAuthenticatedUser()
                                .Build();

                            options.DefaultPolicy = policy;
                            options.FallbackPolicy = policy;
                        });
                });

            });

        return factory;
    }

    public static HttpClient CreateClient()
    {
        var factory = GetFactory();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", bool.TrueString);
        return client;
    }

    public static HttpClient CreateClientAnonymous()
    {
        var factory = GetFactory();
        var client = factory.CreateClient();
        return client;
    }
}

public class AuthenticationTestHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public AuthenticationTestHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        AuthenticateResult result;

        var authHeaderValue = "";
        var authHeader = this.Context.Request.Headers.Authorization.FirstOrDefault();
        if (authHeader is not null)
        {
            authHeaderValue = authHeader.Split(" ").Last();
        }

        if (authHeaderValue == bool.TrueString)
        {
            var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            result = AuthenticateResult.Success(ticket);
        }
        else
        {
            result = AuthenticateResult.Fail("No authorization header");
        }

        return Task.FromResult(result);
    }
}
