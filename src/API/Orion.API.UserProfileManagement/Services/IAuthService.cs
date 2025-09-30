using Orion.API.UserProfileManagement.Models;
using Orion.DataAccess.Postgres.Entities.Common;
using LoginRequest = Orion.API.UserProfileManagement.Models.LoginRequest;

namespace Orion.API.UserProfileManagement.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> RefreshTokenAsync(string token, string refreshToken);
    Task<bool> RevokeTokenAsync(string refreshToken);
}