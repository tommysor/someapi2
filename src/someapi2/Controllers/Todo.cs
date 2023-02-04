namespace someapi2.Controllers;

public sealed class Todo
{
    public Guid Id { get; set; }
    public string Owner { get; set; } = "";
    public string Title { get; set; } = "";
    public bool IsCompleted { get; set; }
}
