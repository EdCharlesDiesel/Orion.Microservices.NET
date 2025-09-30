#nullable enable
using System.ComponentModel.DataAnnotations.Schema;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.Enums;

namespace Orion.DataAccess.Postgres.Entities.Common;
/// <summary>
/// Current basket of the database. 
/// </summary>
[Table("BasketItem")]
public class BasketItem(string? productName) : Entity<Guid>
{
    public string? ProductName { get; set; } = productName;
    private int Quantity { get; set; } = 0;
    public decimal UnitPrice { get; set; }
    public decimal Total => UnitPrice * Quantity;
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeleteDate { get; set; }
    public Status Status { get; set; }
}