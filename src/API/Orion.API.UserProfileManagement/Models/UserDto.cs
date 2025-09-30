namespace Orion.API.UserProfileManagement.Models;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public string? Role { get; set; }
    public bool EmailConfirmed { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public string UserName { get; set; } = string.Empty;

}