using System;

namespace Orion.Domain.DTO;

public class UserStatsDto
{
    public int LoginCount { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime AccountCreated { get; set; }
    public string Role { get; set; } = string.Empty;
}