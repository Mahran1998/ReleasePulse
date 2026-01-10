using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReleasePulse.Api.Data;
using ReleasePulse.Api.Dtos;
using ReleasePulse.Api.Models;

namespace ReleasePulse.Api.Controllers;

[ApiController]
[Route("work-items")]
public class WorkItemsController : ControllerBase
{
    private readonly ReleasePulseDbContext _db;
    public WorkItemsController(ReleasePulseDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkItemDto dto)
    {
        var wi = new WorkItem { Title = dto.Title.Trim(), Description = dto.Description?.Trim() };
        _db.WorkItems.Add(wi);
        await _db.SaveChangesAsync();
        return Created($"/work-items/{wi.Id}", wi);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] WorkItemStatus? status)
    {
        var q = _db.WorkItems.AsNoTracking().OrderByDescending(x => x.CreatedAt);
        if (status is not null) q = q.Where(x => x.Status == status).OrderByDescending(x => x.CreatedAt);
        return Ok(await q.ToListAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var wi = await _db.WorkItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return wi is null ? NotFound() : Ok(wi);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateWorkItemStatusDto dto)
    {
        var wi = await _db.WorkItems.FirstOrDefaultAsync(x => x.Id == id);
        if (wi is null) return NotFound();

        wi.SetStatus(dto.Status);
        await _db.SaveChangesAsync();
        return Ok(wi);
    }

    [HttpPost("{id:guid}/test-cases")]
    public async Task<IActionResult> AddTestCase(Guid id, [FromBody] CreateTestCaseDto dto)
    {
        var exists = await _db.WorkItems.AnyAsync(x => x.Id == id);
        if (!exists) return NotFound();

        var tc = new TestCase { WorkItemId = id, Steps = dto.Steps.Trim(), Expected = dto.Expected.Trim() };
        _db.TestCases.Add(tc);
        await _db.SaveChangesAsync();
        return Created($"/test-cases/{tc.Id}", tc);
    }

    [HttpGet("{id:guid}/test-cases")]
    public async Task<IActionResult> ListTestCases(Guid id)
    {
        var list = await _db.TestCases.AsNoTracking()
            .Where(x => x.WorkItemId == id)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(list);
    }
}
