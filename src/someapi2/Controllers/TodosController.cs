using Microsoft.AspNetCore.Mvc;

namespace someapi2.Controllers;

[ApiController]
[Route("Todos")]
[Produces("application/json")]
public sealed class TodosController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> Get()
    {
        await Task.CompletedTask;
        return Array.Empty<Todo>();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> Get([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
        {
            return new BadRequestResult();
        }
        
        await Task.CompletedTask;
        return new Todo();
    }
}
