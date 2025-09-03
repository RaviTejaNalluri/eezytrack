using EezyTrack.api.Application.Contracts;
using EezyTrack.api.Application.Plans;
using EezyTrack.api.Domain.Entities;
using EezyTrack.api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EezyTrack.api.Endpoints;

public static class GroupsEndpoints
{
    public static RouteGroupBuilder MapGroups(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/groups");

        group.MapPost("", async (ICurrentUser currentUser, AppDbContext db, string name) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == currentUser.Email)
                       ?? db.Users.Add(new User { Email = currentUser.Email, Name = currentUser.Email }).Entity;

            var newGroup = new Group
            {
                Name = name,
                OwnerId = user.Id
            };
            db.Groups.Add(newGroup);
            db.GroupMembers.Add(new GroupMember { Group = newGroup, User = user });
            await db.SaveChangesAsync();
            return Results.Created($"/api/groups/{newGroup.Id}", new { newGroup.Id, newGroup.Name });
        });

        group.MapGet("", async (ICurrentUser currentUser, AppDbContext db) =>
        {
            var groups = await db.Groups
                .Where(g => g.Members.Any(m => m.User!.Email == currentUser.Email))
                .Select(g => new { g.Id, g.Name, g.Plan })
                .ToListAsync();
            return Results.Ok(groups);
        });

        group.MapPost("/{groupId:guid}/invite", async (Guid groupId, string email, AppDbContext db) =>
        {
            var groupEntity = await db.Groups.Include(g => g.Members).FirstOrDefaultAsync(g => g.Id == groupId);
            if (groupEntity == null) return Results.NotFound();
            if (groupEntity.Members.Count >= PlanLimits.MaxMembers(groupEntity.Plan))
            {
                return Results.BadRequest($"Member limit reached for plan {groupEntity.Plan}.");
            }
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email)
                       ?? db.Users.Add(new User { Email = email, Name = email }).Entity;
            if (!groupEntity.Members.Any(m => m.UserId == user.Id))
            {
                db.GroupMembers.Add(new GroupMember { GroupId = groupId, User = user });
                await db.SaveChangesAsync();
            }
            return Results.Ok();
        });

        return group;
    }
}


