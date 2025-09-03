namespace EezyTrack.api.Domain.Entities;

public enum GroupRole
{
    Member = 0,
    Admin = 1,
}

public class GroupMember
{
    public Guid GroupId { get; set; }
    public Group? Group { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public GroupRole Role { get; set; } = GroupRole.Member;
    public DateTime JoinedAtUtc { get; set; } = DateTime.UtcNow;
}


