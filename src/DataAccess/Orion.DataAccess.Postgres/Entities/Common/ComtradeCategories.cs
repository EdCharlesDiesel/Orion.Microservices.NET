using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Entities.Common;

public class ComtradeCategories:Entity<Guid>
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ParentId { get; set; } = null!;
    public string PrettyName { get; set; } = null!;
}