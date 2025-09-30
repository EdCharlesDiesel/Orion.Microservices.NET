using Microsoft.AspNetCore.Identity;
using Orion.DataAccess.Postgres.Services;

namespace Orion.DataAccess.Postgres.Entities.Common;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public IdentityUser User { get; set; } = null!;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}