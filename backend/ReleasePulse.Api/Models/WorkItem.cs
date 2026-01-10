using System.ComponentModel.DataAnnotations;

namespace ReleasePulse.Api.Models;

public class WorkItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public WorkItemStatus Status { get; private set; } = WorkItemStatus.Backlog;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public void SetStatus(WorkItemStatus next)
    {
        // Allowed workflow: Backlog -> InDev -> ReadyForQa -> Done (no skipping)
        if (next == Status) return;

        var ok =
            (Status == WorkItemStatus.Backlog   && next == WorkItemStatus.InDev) ||
            (Status == WorkItemStatus.InDev     && next == WorkItemStatus.ReadyForQa) ||
            (Status == WorkItemStatus.ReadyForQa&& next == WorkItemStatus.Done);

        if (!ok)
            throw new InvalidOperationException($"Invalid transition {Status} -> {next}");

        Status = next;
    }
}
