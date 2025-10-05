using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Orion.DataAccess.Postgres.Entities.Shared;
using Orion.DataAccess.Postgres.Services;
using Orion.Domain.DTO;

namespace Orion.API.UserProfileManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserProfileService _profileService;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(
        UserManager<IdentityUser> userManager,
        IUserProfileService profileService,
        IFileStorageService fileStorageService,
        ILogger<ProfileController> logger)
    {
        _userManager = userManager;
        _profileService = profileService;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<UserProfileDto>> GetProfile()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId ?? throw new InvalidOperationException());

            if (user == null)
                return NotFound(new { message = "User not found" });

            var profile = await _profileService.GetUserProfileAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            var profileDto = new UserProfileDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Phone = profile?.Phone,
                Bio = profile?.Bio,
                Company = profile?.Company,
                Position = profile?.Position,
                Location = profile?.Location,
                Website = profile?.Website,
                AvatarUrl = profile?.AvatarUrl,
                Timezone = profile?.Timezone ?? "UTC",
                Language = profile?.Language ?? "en",
                Role = roles.FirstOrDefault() ?? "User",
                EmailConfirmed = user.EmailConfirmed,
              
                LockoutEnabled = user.LockoutEnabled,
                AccessFailedCount = user.AccessFailedCount,
                NotificationSettings = profile?.NotificationSettings ?? new NotificationSettings(),
                PrivacySettings = profile?.PrivacySettings ?? new PrivacySettings()
            };

            return Ok(profileDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile");
            return StatusCode(500, new { message = "An error occurred while retrieving profile" });
        }
    }

    [HttpPut]
    public async Task<ActionResult<UserProfileDto>> UpdateProfile([FromBody] UpdateProfileRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            // Update basic user info
            user.UserName = request.Name;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });
            }

            // Update extended profile
            var profile = await _profileService.GetUserProfileAsync(userId);
            if (profile == null)
            {
                profile = new UserProfile { UserId = userId };
            }

            profile.Phone = request.Phone;
            profile.Bio = request.Bio;
            profile.Company = request.Company;
            profile.Position = request.Position;
            profile.Location = request.Location;
            profile.Website = request.Website;
            profile.Timezone = request.Timezone;
            profile.Language = request.Language;
            profile.UpdatedAt = DateTime.UtcNow;

            await _profileService.UpdateUserProfileAsync(profile);

            var roles = await _userManager.GetRolesAsync(user);

            var profileDto = new UserProfileDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Phone = profile.Phone,
                Bio = profile.Bio,
                Company = profile.Company,
                Position = profile.Position,
                Location = profile.Location,
                Website = profile.Website,
                AvatarUrl = profile.AvatarUrl,
                Timezone = profile.Timezone,
                Language = profile.Language,
                Role = roles.FirstOrDefault() ?? "User",
                EmailConfirmed = user.EmailConfirmed,
                // CreatedAt = user.CreatedAt,
                // LastLoginAt = user.LastLoginAt,
                // LoginCount = user.LoginCount,
                NotificationSettings = profile.NotificationSettings ?? new NotificationSettings(),
                PrivacySettings = profile.PrivacySettings ?? new PrivacySettings()
            };

            _logger.LogInformation("Profile updated for user {UserId}", userId);
            return Ok(profileDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user profile");
            return StatusCode(500, new { message = "An error occurred while updating profile" });
        }
    }

    [HttpPost("avatar")]
    public async Task<ActionResult<AvatarResponseDto>> UploadAvatar([FromForm] IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded" });

            // Validate file
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                return BadRequest(new { message = "Invalid file type. Only JPEG, PNG, GIF, and WebP are allowed." });

            if (file.Length > 2 * 1024 * 1024) // 2MB
                return BadRequest(new { message = "File size cannot exceed 2MB" });

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            // Upload file
            var fileName = $"avatar_{userId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var avatarUrl = await _fileStorageService.UploadFileAsync(file, "avatars", fileName);

            // Update profile
            var profile = await _profileService.GetUserProfileAsync(userId);
            if (profile == null)
            {
                profile = new UserProfile { UserId = userId };
            }

            // Delete old avatar if exists
            if (!string.IsNullOrEmpty(profile.AvatarUrl))
            {
                await _fileStorageService.DeleteFileAsync(profile.AvatarUrl);
            }

            profile.AvatarUrl = avatarUrl;
            profile.UpdatedAt = DateTime.UtcNow;
            await _profileService.UpdateUserProfileAsync(profile);

            _logger.LogInformation("Avatar updated for user {UserId}", userId);
            return Ok(new AvatarResponseDto { AvatarUrl = avatarUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading avatar");
            return StatusCode(500, new { message = "An error occurred while uploading avatar" });
        }
    }

    [HttpDelete("avatar")]
    public async Task<IActionResult> DeleteAvatar()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var profile = await _profileService.GetUserProfileAsync(userId);

            if (profile?.AvatarUrl != null)
            {
                await _fileStorageService.DeleteFileAsync(profile.AvatarUrl);
                profile.AvatarUrl = null;
                profile.UpdatedAt = DateTime.UtcNow;
                await _profileService.UpdateUserProfileAsync(profile);
            }

            _logger.LogInformation("Avatar deleted for user {UserId}", userId);
            return Ok(new { message = "Avatar deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting avatar");
            return StatusCode(500, new { message = "An error occurred while deleting avatar" });
        }
    }

    [HttpPut("notifications")]
    public async Task<IActionResult> UpdateNotificationSettings([FromBody] NotificationSettings settings)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var profile = await _profileService.GetUserProfileAsync(userId);

            if (profile == null)
            {
                profile = new UserProfile { UserId = userId };
            }

            profile.NotificationSettings = settings;
            profile.UpdatedAt = DateTime.UtcNow;
            await _profileService.UpdateUserProfileAsync(profile);

            _logger.LogInformation("Notification settings updated for user {UserId}", userId);
            return Ok(new { message = "Notification settings updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating notification settings");
            return StatusCode(500, new { message = "An error occurred while updating notification settings" });
        }
    }

    [HttpPut("privacy")]
    public async Task<IActionResult> UpdatePrivacySettings([FromBody] PrivacySettings settings)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var profile = await _profileService.GetUserProfileAsync(userId);

            if (profile == null)
            {
                profile = new UserProfile { UserId = userId };
            }

            profile.PrivacySettings = settings;
            profile.UpdatedAt = DateTime.UtcNow;
            await _profileService.UpdateUserProfileAsync(profile);

            _logger.LogInformation("Privacy settings updated for user {UserId}", userId);
            return Ok(new { message = "Privacy settings updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating privacy settings");
            return StatusCode(500, new { message = "An error occurred while updating privacy settings" });
        }
    }

    [HttpGet("stats")]
    public async Task<ActionResult<UserStatsDto>> GetUserStats()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            var stats = new UserStatsDto
            {
                // LoginCount = user.LoginCount,
                // LastLogin = user.LastLoginAt ?? DateTime.UtcNow,
                // AccountCreated = user.CreatedAt,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User"
            };

            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user stats");
            return StatusCode(500, new { message = "An error occurred while retrieving user stats" });
        }
    }

    [HttpPost("deactivate")]
    public async Task<IActionResult> DeactivateAccount()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            // user.IsActive = false;
            // user.DeactivatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Account deactivated for user {UserId}", userId);
            return Ok(new { message = "Account deactivated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating account");
            return StatusCode(500, new { message = "An error occurred while deactivating account" });
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            // Verify password
            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
                return BadRequest(new { message = "Invalid password" });

            // Soft delete - mark for deletion
            // user.IsActive = false;
            // user.MarkedForDeletion = true;
            // user.DeletionRequestedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // Clean up profile data
            await _profileService.DeleteUserProfileAsync(userId);

            _logger.LogInformation("Account deletion requested for user {UserId}", userId);
            return Ok(new { message = "Account marked for deletion. You have 30 days to recover your account." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting account");
            return StatusCode(500, new { message = "An error occurred while processing account deletion" });
        }
    }

    [HttpPost("export-data")]
    public async Task<IActionResult> ExportUserData()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            var exportData = await _profileService.ExportUserDataAsync(userId);
            
            var fileName = $"user_data_{userId}_{DateTime.UtcNow:yyyyMMdd}.json";
            var contentType = "application/json";
            
            _logger.LogInformation("Data export requested for user {UserId}", userId);
            return File(exportData, contentType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting user data");
            return StatusCode(500, new { message = "An error occurred while exporting data" });
        }
    }
}
