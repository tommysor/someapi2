using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllers();

app.Map("/", (HttpContext httpContext) => 
{
    var body = new { message = "Hello World!" };
    httpContext.Response.ContentType = "application/json; charset=utf-8";
    httpContext.Response.BodyWriter.WriteAsync(JsonSerializer.SerializeToUtf8Bytes(body));
})
    .WithTags("Home")
    ;

app.Run();
