using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orion.API.UserProfileManagement.Models;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities.Shared;
using Orion.DataAccess.Postgres.Services;
using LoginRequest = Orion.API.UserProfileManagement.Models.LoginRequest;

namespace Orion.API.UserProfileManagement.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly OrionDbContext _context;

    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IJwtService jwtService,
        OrionDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _context = context;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !user.EmailConfirmed)
            throw new UnauthorizedAccessException("Invalid credentials");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid credentials");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateToken(user.Id, user.Email!, roles);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Store refresh token
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email!,
                Roles = roles.ToList()
            }
        };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            throw new ArgumentException("Passwords do not match");

        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.Description)));

        // Add default role
        await _userManager.AddToRoleAsync(user, "User");

        return await LoginAsync(new LoginRequest { Email = request.Email, Password = request.Password });
    }

    public async Task<AuthResponse> RefreshTokenAsync(string token, string refreshToken)
    {
        var principal = _jwtService.GetPrincipalFromExpiredToken(token);
        var userId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var storedRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId && !rt.IsRevoked);

        if (storedRefreshToken == null || storedRefreshToken.ExpiryDate <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid refresh token");

        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null || !user.EmailConfirmed)
            throw new UnauthorizedAccessException("User not found");

        var roles = await _userManager.GetRolesAsync(user);
        var newToken = _jwtService.GenerateToken(user.Id, user.Email!, roles);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        // Revoke old refresh token and create new one
        storedRefreshToken.IsRevoked = true;
        var newRefreshTokenEntity = new RefreshToken
        {
            Token = newRefreshToken,
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(newRefreshTokenEntity);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                Roles = roles.ToList()
            }
        };
    }


    public async Task<bool> RevokeTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}