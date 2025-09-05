using System;

namespace TaskManagementSystem.Core.Entities;

public class WorkTask
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkTaskStatus Status { get; set; }
    public DateTime DueDateTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public enum WorkTaskStatus
{
    Todo,
    InProgress,
    Completed
}
