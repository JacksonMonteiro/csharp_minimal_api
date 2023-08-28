using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TaskAPIContext>(options =>
    options.UseInMemoryDatabase("TaskDB")
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Tasks Endpoint
app.MapGet("/tasks", async (TaskAPIContext db) => {
    return await db.Tasks.ToListAsync();
});

app.MapGet("/tasks/{id}", async (int id, TaskAPIContext db) =>
    await db.Tasks.FindAsync(id) is Task task ? Results.Ok(task) : Results.NotFound());

app.MapGet("/tasks/finished", async (TaskAPIContext db) => await db.Tasks.Where(t => t.IsFinished.Value).ToListAsync());

app.MapPost("/tasks", async (Task task, TaskAPIContext db) => {
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{task.Id}", task);
});

app.MapPut("/tasks/{id}", async (int id, Task updatedTask, TaskAPIContext db) => {
    var task = await db.Tasks.FindAsync(id);

    if (task is null) {
        return Results.NotFound();
    }

    task.Name = updatedTask.Name;
    task.IsFinished = updatedTask.IsFinished;
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/tasks/{id}", async (int id, TaskAPIContext db) => {
    if (await db.Tasks.FindAsync(id) is Task task) {
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return Results.Ok(task);
    }

    return Results.NotFound();
});

app.UseHttpsRedirection();
app.Run();

// Classes
class Task {
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool? IsFinished { get; set; }
}

class TaskAPIContext : DbContext {
    public TaskAPIContext(DbContextOptions<TaskAPIContext> options) : base(options) {

    }

    public DbSet<Task> Tasks => Set<Task>();
}