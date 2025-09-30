using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.API.Apps.Controllers;

// [Authorize]
[ApiController]
[Route("[controller]")]
public class RevenueStreamWidgetController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet("GetData")]
    public async Task<IActionResult> GetData()
    {
        try
        {
            var versions = await unitOfWork.RevenueStreams.GetAllAsync();
            return Ok(versions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}