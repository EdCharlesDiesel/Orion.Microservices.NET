using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orion.WebApps.HealthCheckUI.Data;
using Orion.WebApps.HealthCheckUI.Models;

namespace Orion.WebApps.HealthCheckUI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _db;

    public HealthController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _db.HealthRecords.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Add(HealthRecord record)
    {
        _db.HealthRecords.Add(record);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = record.Id }, record);
    }

    [HttpGet("analyze")]
    public async Task<IActionResult> Analyze()
    {
        var risky = await _db.HealthRecords
            .Where(r => r.Bmi > 25 || r.HeartRate > 100)
            .ToListAsync();

        return Ok(new { risky.Count, RiskyProfiles = risky });
    }
}