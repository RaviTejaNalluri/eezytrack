using EezyTrack.api.Application.Plans;
using EezyTrack.api.Domain.Entities;
using EezyTrack.api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EezyTrack.api.Endpoints;

public static class ProjectsEndpoints
{
    public static RouteGroupBuilder MapProjects(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/projects");

        group.MapPost("", async (AppDbContext db, Guid groupId, string name) =>
        {
            var g = await db.Groups.Include(x => x.Projects).FirstOrDefaultAsync(x => x.Id == groupId);
            if (g == null) return Results.NotFound("Group not found");
            if (g.Projects.Count >= PlanLimits.MaxProjects(g.Plan))
            {
                return Results.BadRequest($"Project limit reached for plan {g.Plan}.");
            }
            var project = new Project { Name = name, GroupId = groupId };
            db.Projects.Add(project);
            await db.SaveChangesAsync();
            return Results.Created($"/api/projects/{project.Id}", new { project.Id, project.Name });
        });

        group.MapGet("", async (AppDbContext db, Guid groupId) =>
        {
            var projects = await db.Projects.Where(p => p.GroupId == groupId)
                .Select(p => new { p.Id, p.Name })
                .ToListAsync();
            return Results.Ok(projects);
        });

        return group;
    }
}


