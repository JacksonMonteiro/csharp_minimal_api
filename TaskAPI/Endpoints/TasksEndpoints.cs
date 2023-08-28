using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using TaskAPI.Data;
using static TaskAPI.Data.TaskAPIContext;

namespace TaskAPI.Endpoints {
    public static class TasksEndpoints {
        public static void MapTasksEndpoints(this WebApplication app) {
            app.MapGet("/", () => $"Welcome to Tasks API - {DateTime.Now}");

            app.MapGet("/tasks", async (GetConnection connectionGetter) => {
                using var con = await connectionGetter();
                var tasks = con.GetAll<Data.Task>().ToList();

                if (tasks is null) {
                    return Results.NotFound();
                }

                return Results.Ok(tasks);
            });

            app.MapGet("/tasks/{id}", async (int id, GetConnection connectionGetter) => {
                using var con = await connectionGetter();
                return con.Get<Data.Task>(id) is Data.Task task ? Results.Ok(task) : Results.NotFound();
            });

            app.MapGet("/tasks/finished", async (GetConnection connectionGetter) => {
                using var con = await connectionGetter();
                var tasks = con.Query("SELECT * FROM tasks WHERE isFinished = true");
                if (tasks is null) {
                    return Results.NotFound();
                }
                return Results.Ok(tasks);
            });

            app.MapPost("/tasks", async (Data.Task task, GetConnection connectionGetter) => {
                using var con = await connectionGetter();
                var id = con.Insert(task);
                return Results.Created($"/tasks/{id}", task);
            });

            app.MapPut("/tasks", async (Data.Task updatedTask, GetConnection connectionGetter) => {
                using var con = await connectionGetter();
                var id = con.Update(updatedTask);
                return Results.Ok();
            });

            app.MapDelete("/tasks/{id}", async (int id, GetConnection connectionGetter) => {
                using var con = await connectionGetter();
                var deleted = con.Get<Data.Task>(id);

                if (deleted is null) {
                    return Results.NotFound();
                }

                con.Delete(deleted);
                return Results.Ok(deleted);
            });

        }
    }
}
