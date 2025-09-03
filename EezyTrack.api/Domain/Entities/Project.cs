namespace EezyTrack.api.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid GroupId { get; set; }
    public Group? Group { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}


