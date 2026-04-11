using System;

namespace Orion.Domain.DTO;
/// <summary>
/// 
/// </summary>
public class ChatRequestDto
{
    public string? Message { get; init; } 
    public DateTime? LastUpdate { get; init; }
    public DateTime? Date { get; init; }
    public DateTime? ReferenceDate { get; init; }
}