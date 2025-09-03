using EezyTrack.api.Domain.Enums;

namespace EezyTrack.api.Domain.Entities;

public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
    public User? Owner { get; set; }
    public PlanTier Plan { get; set; } = PlanTier.Free;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}


