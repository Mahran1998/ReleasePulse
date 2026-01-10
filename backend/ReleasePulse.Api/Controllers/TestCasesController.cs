using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReleasePulse.Api.Data;
using ReleasePulse.Api.Dtos;

namespace ReleasePulse.Api.Controllers;

[ApiController]
[Route("test-cases")]
public class TestCasesController : ControllerBase
{
    private readonly ReleasePulseDbContext _db;
    public TestCasesController(ReleasePulseDbContext db) => _db = db;

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTestCaseDto dto)
    {
        var tc = await _db.TestCases.FirstOrDefaultAsync(x => x.Id == id);
        if (tc is null) return NotFound();

        tc.Actual = dto.Actual?.Trim();
        tc.Result = dto.Result;
        tc.TesterNote = dto.TesterNote?.Trim();
        tc.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(tc);
    }
}
