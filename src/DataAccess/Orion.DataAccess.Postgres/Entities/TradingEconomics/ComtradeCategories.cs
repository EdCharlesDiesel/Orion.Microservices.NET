using System.ComponentModel.DataAnnotations.Schema;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Entities.TradingEconomics;
[Table("ComtradeCategories", Schema = "TradingEconomics")]
public class ComtradeCategories:Entity<Guid>
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ParentId { get; set; } = null!;
    public string PrettyName { get; set; } = null!;
}