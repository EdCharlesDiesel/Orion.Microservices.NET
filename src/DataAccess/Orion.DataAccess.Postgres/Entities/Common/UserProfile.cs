using Orion.DataAccess.Postgres.Tools;

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
}