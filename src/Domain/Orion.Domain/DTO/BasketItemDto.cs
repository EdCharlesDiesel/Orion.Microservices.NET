using System;
using Orion.Domain.Enums;

namespace Orion.Domain.DTO;

public class BasketItemDto
{
    public string? ProductName { get; set; }
    private int Quantity { get; set; } = 0;
    public decimal UnitPrice { get; set; }
    public decimal Total => UnitPrice * Quantity;
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeleteDate { get; set; }
    public Status Status { get; set; }
}