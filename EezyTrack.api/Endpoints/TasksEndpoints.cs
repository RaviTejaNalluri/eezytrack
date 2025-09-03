using EezyTrack.api.Domain.Entities;
using EezyTrack.api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EezyTrack.api.Endpoints;

public static class TasksEndpoints
{
    public static RouteGroupBuilder MapTasks(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/tasks");

        group.MapPost("", async (AppDbContext db, Guid projectId, string title, string? description, DateTime? dueDateUtc) =>
        {
            var task = new TaskItem { ProjectId = projectId, Title = title, Description = description, DueDateUtc = dueDateUtc };
            db.Tasks.Add(task);
            await db.SaveChangesAsync();
            return Results.Created($"/api/tasks/{task.Id}", new { task.Id, task.Title });
        });

        group.MapGet("", async (AppDbContext db, Guid projectId) =>
        {
            var tasks = await db.Tasks.Where(t => t.ProjectId == projectId)
                .Select(t => new { t.Id, t.Title, t.Description, t.Status, t.DueDateUtc, t.AssignedToUserId })
                .ToListAsync();
            return Results.Ok(tasks);
        });

        group.MapPost("/{taskId:guid}/assign", async (AppDbContext db, Guid taskId, Guid? userId) =>
        {
            var task = await db.Tasks.FindAsync(taskId);
            if (task == null) return Results.NotFound();
            task.AssignedToUserId = userId;
            task.UpdatedAtUtc = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return Results.Ok();
        });

        group.MapPost("/{taskId:guid}/status", async (AppDbContext db, Guid taskId, TaskStatus status) =>
        {
            var task = await db.Tasks.FindAsync(taskId);
            if (task == null) return Results.NotFound();
            task.Status = status;
            task.UpdatedAtUtc = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return Results.Ok();
        });

        return group;
    }
}


