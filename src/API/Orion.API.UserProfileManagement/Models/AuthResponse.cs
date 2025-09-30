namespace Orion.API.UserProfileManagement.Models;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public UserDto User { get; set; } = new();
}