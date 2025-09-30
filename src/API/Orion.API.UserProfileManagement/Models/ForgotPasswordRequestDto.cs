using System.ComponentModel.DataAnnotations;

namespace Orion.API.UserProfileManagement.Models;

public class ForgotPasswordRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}