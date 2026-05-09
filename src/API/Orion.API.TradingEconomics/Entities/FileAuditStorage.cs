using System.Text.Json;
using Microsoft.Extensions.Options;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Entities;

/// <summary>
/// File-based audit storage for development/small deployments
/// </summary>
public class FileAuditStorage : IAuditStorage
{
    private readonly string _basePath;
    private readonly ILogger<FileAuditStorage> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public FileAuditStorage(
        IOptions<AuditTrailOptions> options, 
        ILogger<FileAuditStorage> logger)
    {
        _logger = logger;
        _basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AuditTrails");
        Directory.CreateDirectory(_basePath);
            
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task StoreBatchAsync(List<AuditEntry> entries)
    {
        var date = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var filePath = Path.Combine(_basePath, $"audit_{date}.jsonl");

        var lines = entries.Select(e => JsonSerializer.Serialize(e, _jsonOptions));
            
        await File.AppendAllLinesAsync(filePath, lines);
        _logger.LogDebug("Stored {Count} entries to {File}", entries.Count, filePath);
    }

    public Task<AuditQueryResult> QueryAsync(AuditQuery query)
    {
        // Implementation would scan files and apply filters
        throw new NotImplementedException(
            "File-based querying requires index implementation. " +
            "Consider using database storage for production.");
    }

    public async Task<AuditEntry> GetByIdAsync(Guid id)
    {
        // Scan recent files for specific entry
        var files = Directory.GetFiles(_basePath, "audit_*.jsonl")
            .OrderByDescending(f => f)
            .Take(7); // Last 7 days

        foreach (var file in files)
        {
            var lines = await File.ReadAllLinesAsync(file);
            foreach (var line in lines)
            {
                var entry = JsonSerializer.Deserialize<AuditEntry>(line, _jsonOptions);
                if (entry?.Id == id)
                    return entry;
            }
        }

        return null;
    }

    public Task ArchiveAsync(DateTime before)
    {
        // Move old files to archive folder
        throw new NotImplementedException();
    }
}