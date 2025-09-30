using System.ComponentModel.DataAnnotations;

namespace Orion.API.UserProfileManagement.Models;

public class RegisterRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;
}