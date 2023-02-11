using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        context =>
        {
            return RateLimitPartition.GetFixedWindowLimiter(
                context.User.Identity?.Name ?? "anonymous",
                fac =>
                {
                    return new FixedWindowRateLimiterOptions
                    {
                        Window = TimeSpan.FromSeconds(1),
                        PermitLimit = 3,
                    };
                });
        }
    );

    options.AddPolicy("fixed", context =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            context.User.Identity?.Name ?? "anonymous",
            fac =>
            {
                return new FixedWindowRateLimiterOptions
                {
                    Window = TimeSpan.FromSeconds(1),
                    PermitLimit = 3,
                };
            });
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001";
        options.Audience = "someapi2";
    });

builder.Services.AddAuthorization(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();

    options.DefaultPolicy = policy;
    options.FallbackPolicy = policy;
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRateLimiter();

app.UseHsts();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    await next();
});

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireRateLimiting("fixed")
    .RequireAuthorization()
    ;

app.Map("/", (HttpContext httpContext) => 
{
    var body = new { message = "Hello World!" };
    httpContext.Response.ContentType = "application/json; charset=utf-8";
    httpContext.Response.BodyWriter.WriteAsync(JsonSerializer.SerializeToUtf8Bytes(body));
})
    .WithTags("Home")
    .RequireRateLimiting("fixed")
    .RequireAuthorization()
    .AllowAnonymous()
    ;

app.Run();
