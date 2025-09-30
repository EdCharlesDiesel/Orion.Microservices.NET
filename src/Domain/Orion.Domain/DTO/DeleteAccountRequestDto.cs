using System.ComponentModel.DataAnnotations;

namespace Orion.Domain.DTO;

public class DeleteAccountRequestDto
{
    [Required]
    public string Password { get; set; } = string.Empty;
}