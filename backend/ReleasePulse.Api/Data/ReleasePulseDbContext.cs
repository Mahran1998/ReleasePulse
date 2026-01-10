using Microsoft.EntityFrameworkCore;
using ReleasePulse.Api.Models;

namespace ReleasePulse.Api.Data;

public class ReleasePulseDbContext : DbContext
{
    public ReleasePulseDbContext(DbContextOptions<ReleasePulseDbContext> options) : base(options) {}

    public DbSet<WorkItem> WorkItems => Set<WorkItem>();
    public DbSet<TestCase> TestCases => Set<TestCase>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkItem>()
            .Property(w => w.Status)
            .HasConversion<string>();

        modelBuilder.Entity<TestCase>()
            .Property(t => t.Result)
            .HasConversion<string>();

        modelBuilder.Entity<TestCase>()
            .HasOne(t => t.WorkItem)
            .WithMany()
            .HasForeignKey(t => t.WorkItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
