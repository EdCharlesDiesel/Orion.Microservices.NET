using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities.Common;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Services;

public class UserProfileService : IUserProfileService
{
    private readonly OrionDbContext _context;
    private readonly ILogger<UserProfileService> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public UserProfileService(
        OrionDbContext context,
        ILogger<UserProfileService> logger,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<UserProfile?> GetUserProfileAsync(string userId)
    {
        try
        {
            return await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile for user {UserId}", userId);
            throw;
        }
    }

    public async Task<UserProfile> CreateUserProfileAsync(UserProfile profile)
    {
        try
        {
            profile.CreatedAt = DateTime.UtcNow;
            profile.UpdatedAt = DateTime.UtcNow;

            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User profile created for user {UserId}", profile.UserId);
            return profile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user profile for user {UserId}", profile.UserId);
            throw;
        }
    }

    public async Task<UserProfile> UpdateUserProfileAsync(UserProfile profile)
    {
        try
        {
            var existingProfile = await GetUserProfileAsync(profile.UserId);
            
            if (existingProfile == null)
            {
                return await CreateUserProfileAsync(profile);
            }

            // Update properties
            existingProfile.Phone = profile.Phone;
            existingProfile.Bio = profile.Bio;
            existingProfile.Company = profile.Company;
            existingProfile.Position = profile.Position;
            existingProfile.Location = profile.Location;
            existingProfile.Website = profile.Website;
            existingProfile.AvatarUrl = profile.AvatarUrl ?? existingProfile.AvatarUrl;
            existingProfile.Timezone = profile.Timezone;
            existingProfile.Language = profile.Language;
            existingProfile.NotificationSettings = profile.NotificationSettings ?? existingProfile.NotificationSettings;
            existingProfile.PrivacySettings = profile.PrivacySettings ?? existingProfile.PrivacySettings;
            existingProfile.UpdatedAt = DateTime.UtcNow;

            _context.UserProfiles.Update(existingProfile);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User profile updated for user {UserId}", profile.UserId);
            return existingProfile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user profile for user {UserId}", profile.UserId);
            throw;
        }
    }

    public async Task DeleteUserProfileAsync(string userId)
    {
        try
        {
            var profile = await GetUserProfileAsync(userId);
            if (profile != null)
            {
                _context.UserProfiles.Remove(profile);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User profile deleted for user {UserId}", userId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user profile for user {UserId}", userId);
            throw;
        }
    }

   // [RequiresUnreferencedCode()]
    public async Task<byte[]> ExportUserDataAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            var profile = await GetUserProfileAsync(userId);
            var roles = user != null ? await _userManager.GetRolesAsync(user) : new List<string>();

            var exportData = new
            {
                User = new
                {
                    user?.Id,
                    user?.UserName,
                    user?.Email,
                    // user?.CreatedAt,
                    // user?.LastLoginAt,
                    // user?.LoginCount,
                    EmailConfirmed = user?.EmailConfirmed ?? false,
                    Roles = roles
                },
                Profile = profile != null ? new
                {
                    profile.Phone,
                    profile.Bio,
                    profile.Company,
                    profile.Position,
                    profile.Location,
                    profile.Website,
                    profile.AvatarUrl,
                    profile.Timezone,
                    profile.Language,
                    profile.NotificationSettings,
                    profile.PrivacySettings,
                    profile.CreatedAt,
                    profile.UpdatedAt
                } : null,
                ExportedAt = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            _logger.LogInformation("User data exported for user {UserId}", userId);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting user data for user {UserId}", userId);
            throw;
        }
    }

    public async Task<List<UserProfile>> GetUserProfilesByRoleAsync(string role)
    {
        try
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            var userIds = usersInRole.Select(u => u.Id).ToList();

            return await _context.UserProfiles
                .Where(p => userIds.Contains(p.UserId))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profiles for role {Role}", role);
            throw;
        }
    }

    public async Task<bool> UserProfileExistsAsync(string userId)
    {
        try
        {
            return await _context.UserProfiles
                .AnyAsync(p => p.UserId == userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if user profile exists for user {UserId}", userId);
            throw;
        }
    }
}