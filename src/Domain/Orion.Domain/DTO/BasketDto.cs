using System.Collections.Generic;

namespace Orion.Domain.DTO;

public class BasketDto
{
    public List<BasketItemDto>? Items { get; set; } 
    public decimal? TotalPrice { get; set; } 
    public string? Currency { get; set; } 
    public bool IsCheckedOut { get; set; } 
}