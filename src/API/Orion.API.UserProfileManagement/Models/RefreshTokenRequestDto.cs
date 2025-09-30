using System.ComponentModel.DataAnnotations;

namespace Orion.API.UserProfileManagement.Models;

public class RefreshTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}