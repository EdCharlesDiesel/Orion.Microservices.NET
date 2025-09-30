using System.ComponentModel.DataAnnotations;

namespace Orion.Domain.DTO;

public class NotificationSettings
{
    [Key]
    public int Id { get; set; }
    public bool Email { get; set; } = true;
    public bool Push { get; set; } = true;
    public bool Sms { get; set; } = false;
    public bool Marketing { get; set; } = false;
}