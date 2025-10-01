using Orion.DataAccess.Postgres.Entities.Shared;

namespace Orion.DataAccess.Postgres.Services;

public interface IUserProfileService
{

        Task<UserProfile?> GetUserProfileAsync(string userId);
        Task<UserProfile> CreateUserProfileAsync(UserProfile profile);
        Task<UserProfile> UpdateUserProfileAsync(UserProfile profile);
        Task DeleteUserProfileAsync(string userId);
        Task<byte[]> ExportUserDataAsync(string userId);
        Task<List<UserProfile>> GetUserProfilesByRoleAsync(string role);
        Task<bool> UserProfileExistsAsync(string userId);
}