using System.ComponentModel.DataAnnotations;

namespace ReleasePulse.Api.Models;

public class TestCase
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid WorkItemId { get; set; }
    public WorkItem? WorkItem { get; set; }

    [Required, MaxLength(4000)]
    public string Steps { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string Expected { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Actual { get; set; }

    public TestResult Result { get; set; } = TestResult.NotRun;

    [MaxLength(2000)]
    public string? TesterNote { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
