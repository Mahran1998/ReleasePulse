using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReleasePulse.Api.Data;
using ReleasePulse.Api.Models;

namespace ReleasePulse.Api.Controllers;

[ApiController]
[Route("reports")]
public class ReportsController : ControllerBase
{
    private readonly ReleasePulseDbContext _db;
    public ReportsController(ReleasePulseDbContext db) => _db = db;

    [HttpGet("release")]
    public async Task<IActionResult> ReleaseReport()
    {
        var workItems = await _db.WorkItems.AsNoTracking().ToListAsync();
        var tests = await _db.TestCases.AsNoTracking().ToListAsync();

        var total = tests.Count;
        var passed = tests.Count(t => t.Result == TestResult.Pass);
        var failed = tests.Count(t => t.Result == TestResult.Fail);
        var notRun = tests.Count(t => t.Result == TestResult.NotRun);
        var passRate = total == 0 ? 0 : Math.Round((double)passed / total * 100, 2);

        var byStatus = workItems
            .GroupBy(w => w.Status.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        return Ok(new
        {
            workItemsTotal = workItems.Count,
            workItemsByStatus = byStatus,
            manualTests = new { total, passed, failed, notRun, passRatePercent = passRate },
            automation = new { latestSummaryPath = "/test-summary.json" }
        });
    }

    [HttpGet("release.csv")]
    public async Task<IActionResult> ExportCsv()
    {
        var tests = await _db.TestCases.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("Id,WorkItemId,Result,Steps,Expected,Actual,TesterNote,CreatedAt,UpdatedAt");

        static string esc(string? s) => "\"" + (s ?? "").Replace("\"", "\"\"") + "\"";

        foreach (var t in tests)
        {
            sb.AppendLine(string.Join(",",
                t.Id,
                t.WorkItemId,
                t.Result,
                esc(t.Steps),
                esc(t.Expected),
                esc(t.Actual),
                esc(t.TesterNote),
                t.CreatedAt,
                t.UpdatedAt
            ));
        }

        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "release-report.csv");
    }
}
