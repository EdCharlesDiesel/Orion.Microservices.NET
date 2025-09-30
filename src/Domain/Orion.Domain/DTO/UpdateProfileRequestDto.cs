using System.ComponentModel.DataAnnotations;

namespace Orion.Domain.DTO;

public class UpdateProfileRequestDto
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;

    [Phone]
    public string? Phone { get; set; }

    [MaxLength(500)]
    public string? Bio { get; set; }

    public string? Company { get; set; }
    public string? Position { get; set; }
    public string? Location { get; set; }

    [Url]
    public string? Website { get; set; }

    public string Timezone { get; set; } = "UTC";
    public string Language { get; set; } = "en";
}