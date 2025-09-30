using Microsoft.AspNetCore.Mvc;

using Orion.API.UserProfile.Models;
using Orion.DataAccess.Postgres.Data;

namespace Orion.API.UserProfile.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(OrionDbContext dbContext, JwtService jwtService) : ControllerBase
{
    private readonly JwtService _jwtService = jwtService;

    //TODO: This needs to be fixed.
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        throw new NotImplementedException();
        // if (_dbContext.UserProfiles.Any(u => u.Username == request.Username))
        //     return BadRequest("Username already exists.");
        //
        // var user = new Core.UserProfile.Domain.UserProfile
        // {
        //     Id = Guid.NewGuid(),
        //     Username = request.Username,
        //     Email = request.Email,
        //     // PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
        //     Role = request.Role
        // };
        //
        // _dbContext.UserProfiles.Add(user);
        // await dbContext.SaveChangesAsync();
        //
        // return Ok("User registered successfully.");
    }
    
    //TODO: This needs to be fixed.
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        // var user = _dbContext.UserProfiles.SingleOrDefault(u => u.Username == request.Username);
        
        //TODO: We need to fix this piece of code and 
        // if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        //     return Unauthorized("Invalid credentials.");

        // var token = _jwtService.GenerateToken(user);
        // return Ok(new { token });
        throw new NotImplementedException();
    }
}

public class JwtService
{
}