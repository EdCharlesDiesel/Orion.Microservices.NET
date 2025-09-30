namespace Orion.DataAccess.Postgres.Entities.Common;

public class RegisterUserRequest
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = "User";
}