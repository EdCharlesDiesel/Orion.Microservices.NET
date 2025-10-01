using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities.Shared;

[NotMapped]
public class RevenueStream
{
    private decimal UnitPrice { get; set; }
    private DateTime ModifiedDate { get; set; }
}