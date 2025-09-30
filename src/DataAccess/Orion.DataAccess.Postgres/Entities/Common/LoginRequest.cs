namespace Orion.DataAccess.Postgres.Entities.Common;

public class LoginRequest
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}