using System.Security.Claims;

namespace Orion.DataAccess.Postgres.Services;

public interface IJwtService
{
    string GenerateToken(string userId, string email, IEnumerable<string> roles);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    bool ValidateToken(string token);
}