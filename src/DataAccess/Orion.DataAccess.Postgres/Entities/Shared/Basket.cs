#nullable enable
using System.ComponentModel.DataAnnotations.Schema;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Entities.Shared
{
    /// <summary>
    /// Current basket of the database. 
    /// </summary>
    [Table("Basket", Schema = "Shared")]
    public class Basket:Entity<Guid>
    {
        public Guid UserId { get; set; } 
        public List<BasketItem>? Items { get; set; } 
        public decimal? TotalPrice { get; set; } 
        public string? Currency { get; set; } 
        public bool IsCheckedOut { get; set; }

    }
}
