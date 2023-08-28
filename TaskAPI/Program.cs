using TaskAPI.Endpoints;
using TaskAPI.Extensions;

// Builder configurations

var builder = WebApplication.CreateBuilder(args);
builder.AddPersistence();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// App Configurations

var app = builder.Build();


if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapTasksEndpoints();
app.Run();