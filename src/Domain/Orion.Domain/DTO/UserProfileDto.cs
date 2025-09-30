using System;

namespace Orion.Domain.DTO;

public class UserProfileDto
{
    public string Id { get; set; } = string.Empty;
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
    public string Role { get; set; } = "User";
    public bool EmailConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public int LoginCount { get; set; }
    public NotificationSettings NotificationSettings { get; set; } = new();
    public PrivacySettings PrivacySettings { get; set; } = new();
}