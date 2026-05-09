using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.DTO;

namespace Orion.DataAccess.Postgres.Entities.Common;

public class UserProfile: Entity<Guid>
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string Role { get; set; } = "User";
    public string? FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; } = default!;
    public string? Subscription  { get; set; }  = default!;
    public string? UserTypeId  { get; set; }  = default!;
    public string? IsLoggedIn  { get; set; }  = default!;
    public string? Nickname  { get; set; }  = default!;
    public Guid? Code  { get; set; } = Guid.NewGuid();
    public string? Image  { get; set; }  = default!;
    public string UserId { get; set; }
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public string? Company { get; set; }
    public NotificationSettings NotificationSettings { get; set; }
    public PrivacySettings PrivacySettings { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Language { get; set; }
    public string Timezone { get; set; }
    public string AvatarUrl { get; set; }
    public string? Website { get; set; }
    public string? Location { get; set; }
    public string? Position { get; set; }
    public string CreatedAt { get; set; }
}