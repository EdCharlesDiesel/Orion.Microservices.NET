using System;

namespace Orion.Domain.DTO;

public class ProductDto
{
    public Guid UserId { get; set; } 
    public string? OrderNumber { get; set; } 
    public int? OrderId { get; set; } 
    public DateTime OrderDate { get; set; } 
}