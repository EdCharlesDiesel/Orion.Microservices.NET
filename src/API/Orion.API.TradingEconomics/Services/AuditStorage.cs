using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Services;

public sealed class AuditStorage : IAuditStorage
{
    private readonly List<AuditEntry> _entries = [];
    private readonly object _lock = new();

    public Task StoreBatchAsync(List<AuditEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        lock (_lock)
        {
            foreach (var entry in entries)
            {
                if (entry.Id == Guid.Empty)
                    entry.Id = Guid.NewGuid();

                _entries.Add(entry);
            }
        }

        return Task.CompletedTask;
    }

    public Task<AuditQueryResult> QueryAsync(AuditQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        List<AuditEntry> result;

        lock (_lock)
        {
            result = _entries
                .Where(x => query.StartDate == null || x.TimestampUtc >= query.StartDate)
                .Where(x => query.EndDate == null || x.TimestampUtc <= query.EndDate)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();
        }

        return Task.FromResult(new AuditQueryResult
        {
            Entries = result,
            TotalCount = result.Count,
            Page = query.Page,
            PageSize = query.PageSize
        });
    }

    public Task<AuditEntry> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Audit id is required.", nameof(id));

        lock (_lock)
        {
            var entry = _entries.FirstOrDefault(x => x.Id == id);

            if (entry == null)
                throw new KeyNotFoundException($"Audit entry not found: {id}");

            return Task.FromResult(entry);
        }
    }

    public Task ArchiveAsync(DateTime before)
    {
        lock (_lock)
        {
            _entries.RemoveAll(x => x.TimestampUtc < before);
        }

        return Task.CompletedTask;
    }
}