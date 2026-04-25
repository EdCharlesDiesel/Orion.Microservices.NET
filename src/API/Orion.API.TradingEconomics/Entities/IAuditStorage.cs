using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine;

public interface IAuditStorage
{
    Task StoreBatchAsync(List<AuditEntry> entries);
    Task<AuditQueryResult> QueryAsync(AuditQuery query);
    Task<AuditEntry> GetByIdAsync(Guid id);
    Task ArchiveAsync(DateTime before);
}