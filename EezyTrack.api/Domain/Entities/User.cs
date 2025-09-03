namespace EezyTrack.api.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Name { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();
    public ICollection<Group> OwnedGroups { get; set; } = new List<Group>();
}


