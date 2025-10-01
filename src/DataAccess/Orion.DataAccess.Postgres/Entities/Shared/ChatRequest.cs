#nullable enable
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Entities.Shared;

public class ChatRequest:Entity<Guid>
{
    public string? Message { get; set; } 
    public DateTime? LastUpdate { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? ReferenceDate { get; set; }
}