using System.ComponentModel.DataAnnotations;

namespace Orion.API.UserProfileManagement.Models;

public class ResetPasswordRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
}