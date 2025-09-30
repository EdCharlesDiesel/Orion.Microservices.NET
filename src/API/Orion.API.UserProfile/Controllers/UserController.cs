using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.DataAccess.Postgres.Data;

namespace Orion.API.UserProfile.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly OrionDbContext _dbContext;

    public UserController(OrionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // if (userId == null) return Unauthorized();
        //
        // var user = _dbContext.UserProfiles.Find(Guid.Parse(userId));
        // if (user == null) return NotFound();
        //
        // return Ok(new
        // {
        //     user.Id,
        //     user.Username,
        //     user.Email,
        //     user.Role,
        //     user.FirstName,
        //     user.LastName,
        //     user.DateOfBirth
        // });
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public IActionResult GetAllUsers()
    {
        // return Ok(_dbContext.UserProfiles.ToList());
        throw new NotImplementedException();
    }
}