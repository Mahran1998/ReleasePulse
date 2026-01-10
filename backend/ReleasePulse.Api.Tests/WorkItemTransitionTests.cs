using FluentAssertions;
using ReleasePulse.Api.Models;
using Xunit;

public class WorkItemTransitionTests
{
    [Fact]
    public void Valid_transitions_work()
    {
        var w = new WorkItem { Title = "Test" };

        w.SetStatus(WorkItemStatus.InDev);
        w.SetStatus(WorkItemStatus.ReadyForQa);
        w.SetStatus(WorkItemStatus.Done);

        w.Status.Should().Be(WorkItemStatus.Done);
    }

    [Fact]
    public void Skipping_status_is_rejected()
    {
        var w = new WorkItem { Title = "Test" };

        var act = () => w.SetStatus(WorkItemStatus.ReadyForQa);
        act.Should().Throw<InvalidOperationException>();
    }
}
