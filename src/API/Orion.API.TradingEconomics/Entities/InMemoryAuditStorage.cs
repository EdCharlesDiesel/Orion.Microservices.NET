using System.Collections.Concurrent;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine;

/// <summary>
/// In-memory storage for testing
/// </summary>
public class InMemoryAuditStorage : IAuditStorage
{
    private readonly ConcurrentDictionary<Guid, AuditEntry> _store = new();

    public Task StoreBatchAsync(List<AuditEntry> entries)
    {
        foreach (var entry in entries)
        {
            _store[entry.Id] = entry;
        }
        return Task.CompletedTask;
    }

    public Task<AuditQueryResult> QueryAsync(AuditQuery query)
    {
        var filtered = _store.Values.AsEnumerable();

        if (query.StartDate.HasValue)
            filtered = filtered.Where(e => e.Timestamp >= query.StartDate.Value);
            
        if (query.EndDate.HasValue)
            filtered = filtered.Where(e => e.Timestamp <= query.EndDate.Value);
            
        if (!string.IsNullOrEmpty(query.Pair))
            filtered = filtered.Where(e => e.Pair == query.Pair);
            
        if (query.RecordType.HasValue)
            filtered = filtered.Where(e => e.RecordType == query.RecordType.Value);

        var totalCount = filtered.Count();
        var entries = filtered
            .OrderByDescending(e => e.Timestamp)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        return Task.FromResult(new AuditQueryResult
        {
            Entries = entries,
            TotalCount = totalCount,
            PageSize = query.PageSize,
            PageNumber = query.PageNumber,
            HasMorePages = (query.PageNumber * query.PageSize) < totalCount
        });
    }

    public Task<AuditEntry> GetByIdAsync(Guid id)
    {
        _store.TryGetValue(id, out var entry);
        return Task.FromResult(entry);
    }

    public Task ArchiveAsync(DateTime before)
    {
        var toArchive = _store.Values
            .Where(e => e.Timestamp < before)
            .Select(e => e.Id)
            .ToList();

        foreach (var id in toArchive)
        {
            _store.TryRemove(id, out _);
        }

        return Task.CompletedTask;
    }
}