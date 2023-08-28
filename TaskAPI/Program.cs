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

app.MapPost("/tasks", async (Task task, TaskAPIContext db) => {
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{task.Id}", task);
});

app.UseHttpsRedirection();
app.Run();

// Classes
class Task {
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool? isFinished { get; set; }
}

class TaskAPIContext : DbContext {
    public TaskAPIContext(DbContextOptions<TaskAPIContext> options) : base(options) {

    }

    public DbSet<Task> Tasks => Set<Task>();
}