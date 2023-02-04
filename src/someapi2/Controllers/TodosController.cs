using Microsoft.AspNetCore.Mvc;

namespace someapi2.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class TodosController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> Get()
    {
        await Task.CompletedTask;
        return Array.Empty<Todo>();
    }

}
