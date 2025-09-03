namespace EezyTrack.api.Domain.Entities;

public enum TaskStatus
{
    ToDo = 0,
    InProgress = 1,
    Done = 2
}

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDateUtc { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.ToDo;

    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }

    public Guid? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
}


