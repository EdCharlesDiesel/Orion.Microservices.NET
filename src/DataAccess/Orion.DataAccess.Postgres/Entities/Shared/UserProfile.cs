using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Orion.Domain.DTO;

namespace Orion.DataAccess.Postgres.Entities.Shared;
[Table("UserProfile", Schema = "Shared")]
public class UserProfile
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; }
    public string Username { get; set; } = default!;
    
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public string? Company { get; set; }
    public string? Position { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public string? AvatarUrl { get; set; }
    public string Timezone { get; set; } = "UTC";
    public string Language { get; set; } = "en";
    public string? Nickname  { get; set; }  = default!;
    public string Role { get; set; } = "User";
    public bool EmailConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int LoginCount { get; set; }
    public DateTime? DateOfBirth { get; set; } = default!;
    public string? Subscription  { get; set; }  = default!;
    public string? UserTypeId  { get; set; }  = default!;
    public string? IsLoggedIn  { get; set; }  = default!;
    public NotificationSettings NotificationSettings { get; set; } = new();
    public PrivacySettings PrivacySettings { get; set; } = new();

    // Navigation property
    public virtual IdentityUser User { get; set; } = null!;
    
}