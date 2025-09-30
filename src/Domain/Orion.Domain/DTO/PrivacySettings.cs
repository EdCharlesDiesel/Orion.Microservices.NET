using System.ComponentModel.DataAnnotations;

namespace Orion.Domain.DTO;

public class PrivacySettings
{
    [Key]
    public int Id { get; set; }
    public bool ProfileVisible { get; set; } = true;
    public bool ShowEmail { get; set; } = false;
    public bool ShowPhone { get; set; } = false;
}