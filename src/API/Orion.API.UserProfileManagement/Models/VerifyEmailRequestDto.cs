using System.ComponentModel.DataAnnotations;

namespace Orion.API.UserProfileManagement.Models;

public class VerifyEmailRequestDto
{
    [Required]
    public string Token { get; set; } = string.Empty;
}