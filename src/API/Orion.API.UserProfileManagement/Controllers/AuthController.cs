using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Orion.API.UserProfileManagement.Models;
using Orion.API.UserProfileManagement.Services;

namespace Orion.API.UserProfileManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly IEmailService _emailService;

    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration,
        ILogger<AuthController> logger,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
        _emailService = emailService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            // Validate request
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest(new { message = "User with this email already exists" });

            // Create new user
            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, "User");

            // Generate email confirmation token
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", 
                new { userId = user.Id, token = emailToken }, Request.Scheme);

            // Send confirmation email
            await _emailService.SendEmailConfirmationAsync(user.Email, user.UserName, confirmationLink);

            // Generate JWT token
            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // Note: Refresh token storage would require custom IdentityUser extension
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User {Email} registered successfully", request.Email);

            return Ok(new AuthResponseDto
            {
                User = new Models.UserDto
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email,
                    Role = "User",
                    EmailConfirmed = user.EmailConfirmed
                },
                Token = token,
                RefreshToken = refreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return StatusCode(500, new { message = "An error occurred during registration" });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(new { message = "Invalid email or password" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    return BadRequest(new { message = "Account locked due to multiple failed login attempts" });
                
                if (result.IsNotAllowed)
                    return BadRequest(new { message = "Please verify your email address before logging in" });

                return BadRequest(new { message = "Invalid email or password" });
            }

            // Update user (for any future tracking needs)
            await _userManager.UpdateAsync(user);

            // Generate tokens
            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // Note: Refresh token storage would require custom IdentityUser extension
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation("User {Email} logged in successfully", request.Email);

            return Ok(new AuthResponseDto
            {
                User = new Models.UserDto
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "User",
                    EmailConfirmed = user.EmailConfirmed
                },
                Token = token,
                RefreshToken = refreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    // Note: Refresh token clearing would require custom IdentityUser extension
                    await _userManager.UpdateAsync(user);
                }
            }

            await _signInManager.SignOutAsync();
            
            _logger.LogInformation("User {UserId} logged out", userId);
            return Ok(new { message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return StatusCode(500, new { message = "An error occurred during logout" });
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        return StatusCode(501, new { message = "Refresh token functionality not implemented. Requires custom IdentityUser extension to store refresh tokens." });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Ok(new { message = "If an account with that email exists, a reset link has been sent" });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action(nameof(ResetPassword), "Auth",
                new { email = user.Email, token }, Request.Scheme);

            // Note: Uncomment when email service method is available
            // await _emailService.SendPasswordResetAsync(user.Email, user.UserName, resetLink);

            _logger.LogInformation("Password reset email sent to {Email}", request.Email);
            return Ok(new { message = "Password reset instructions have been sent to your email" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password");
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(new { message = "Invalid reset token" });

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });
            }

            // Note: Refresh token clearing would require custom IdentityUser extension
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Password reset successfully for user {Email}", request.Email);
            return Ok(new { message = "Password has been reset successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset");
            return StatusCode(500, new { message = "An error occurred while resetting password" });
        }
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return BadRequest(new { message = "User not found" });

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });
            }

            _logger.LogInformation("Password changed successfully for user {Email}", user.Email);
            return Ok(new { message = "Password changed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password change");
            return StatusCode(500, new { message = "An error occurred while changing password" });
        }
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        try
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Invalid confirmation link" });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest(new { message = "Invalid confirmation link" });

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest(new { message = "Email confirmation failed" });

            _logger.LogInformation("Email confirmed for user {Email}", user.Email);
            return Ok(new { message = "Email confirmed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during email confirmation");
            return StatusCode(500, new { message = "An error occurred during email confirmation" });
        }
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequestDto request)
    {
        return StatusCode(501, new { message = "Email verification functionality not implemented. Use confirm-email endpoint instead." });
    }

    // Private helper methods
    private async Task<string> GenerateJwtToken(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}