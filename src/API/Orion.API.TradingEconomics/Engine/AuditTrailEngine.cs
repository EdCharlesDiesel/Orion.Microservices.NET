using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine;

/// <summary>
/// Records and stores audit events for trading decisions, pipeline steps, errors, and system events.
/// </summary>
public sealed class AuditTrailEngine : IAuditTrailEngine
{
    private readonly ILogger<AuditTrailEngine> _logger;
    private readonly AuditTrailOptions _options;
    private readonly ConcurrentQueue<AuditEntry> _buffer = new();
    private readonly SemaphoreSlim _flushLock = new(1, 1);
    private readonly IAuditStorage _storage;
    private DateTime _lastFlushTime = DateTime.UtcNow;
    private int _sequenceNumber;

    public AuditTrailEngine(ILogger<AuditTrailEngine> logger, IOptions<AuditTrailOptions> options, IAuditStorage storage)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? new AuditTrailOptions();
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    /// <summary>
    /// Records a full trading decision audit record.
    /// </summary>
    public async Task<Guid> RecordDecisionAsync(AuditRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);

        var entry = CreateEntry(
            AuditRecordType.Decision,
            record.CorrelationId,
            record);

        entry.SessionId = record.SessionId;
        entry.Pair = record.Input?.Pair;
        entry.Direction = record.Decision?.Direction;
        entry.Confidence = record.Decision?.Confidence;

        await EnqueueAndMaybeFlushAsync(entry);
        return entry.Id;
    }

    /// <summary>
    /// Records a single pipeline step.
    /// </summary>
    public Task RecordPipelineStepAsync<T>(Guid correlationId, string stepName, T stepData, TimeSpan duration)
    {
        if (string.IsNullOrWhiteSpace(stepName))
            throw new ArgumentException("Step name is required.", nameof(stepName));

        var entry = CreateEntry(
            AuditRecordType.PipelineStep,
            correlationId,
            new PipelineStepAudit
            {
                StepName = stepName.Trim(),
                Data = stepData,
                Duration = duration,
                DataType = typeof(T).Name
            });

        return EnqueueAndMaybeFlushAsync(entry);
    }

    /// <summary>
    /// Records an execution error.
    /// </summary>
    public Task RecordErrorAsync(Guid correlationId, string stage, Exception exception, Dictionary<string, object>? context = null)
    {
        if (string.IsNullOrWhiteSpace(stage))
            throw new ArgumentException("Stage is required.", nameof(stage));

        ArgumentNullException.ThrowIfNull(exception);

        var entry = CreateEntry(
            AuditRecordType.Error,
            correlationId,
            new ErrorAudit
            {
                Stage = stage.Trim(),
                ExceptionType = exception.GetType().Name,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Context = context ?? new Dictionary<string, object>()
            });

        return EnqueueAndMaybeFlushAsync(entry);
    }

    /// <summary>
    /// Records a business or system event.
    /// </summary>
    public Task RecordEventAsync(Guid correlationId, string eventName, Dictionary<string, object>? metadata = null)
    {
        if (string.IsNullOrWhiteSpace(eventName))
            throw new ArgumentException("Event name is required.", nameof(eventName));

        var entry = CreateEntry(
            AuditRecordType.Event,
            correlationId,
            new EventAudit
            {
                EventName = eventName.Trim(),
                Metadata = metadata ?? new Dictionary<string, object>()
            });

        return EnqueueAndMaybeFlushAsync(entry);
    }

    /// <summary>
    /// Flushes buffered audit records to storage.
    /// </summary>
    public async Task FlushAsync()
    {
        await _flushLock.WaitAsync();

        var entries = new List<AuditEntry>();

        try
        {
            while (_buffer.TryDequeue(out var entry))
                entries.Add(entry);

            if (entries.Count == 0)
                return;

            await _storage.StoreBatchAsync(entries);

            _lastFlushTime = DateTime.UtcNow;

            _logger.LogDebug("Flushed {Count} audit entries to storage.", entries.Count);
        }
        catch (Exception ex)
        {
            foreach (var entry in entries)
                _buffer.Enqueue(entry);

            _logger.LogError(ex, "Failed to flush audit entries. Entries were re-queued.");
        }
        finally
        {
            _flushLock.Release();
        }
    }

    /// <summary>
    /// Queries audit records from storage.
    /// </summary>
    public Task<AuditQueryResult> QueryAsync(AuditQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);
        return _storage.QueryAsync(query);
    }

    /// <summary>
    /// Generates a compliance report from stored decision audit records.
    /// </summary>
    public async Task<ComplianceReport> GenerateComplianceReportAsync(DateTime startDate, DateTime endDate, string? pair = null)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));

        var result = await _storage.QueryAsync(new AuditQuery
        {
            StartDate = startDate,
            EndDate = endDate,
            Pair = pair,
            RecordType = AuditRecordType.Decision
        });

        var decisionEntries = result.Entries
            .Where(x => x.RecordType == AuditRecordType.Decision)
            .ToList();

        var decisions = decisionEntries
            .Select(x => x.Data)
            .OfType<AuditRecord>()
            .Where(x => x.Decision != null)
            .Select(x => x.Decision!)
            .ToList();

        return new ComplianceReport
        {
            GeneratedAt = DateTime.UtcNow,
            Period = new DateRange(startDate, endDate),
            Pair = pair,
            TotalDecisions = result.TotalCount,
            TradesByDirection = decisions
                .GroupBy(x => x.Direction)
                .ToDictionary(x => x.Key, x => x.Count()),
            AverageConfidence = decisions.Count == 0
                ? 0
                : decisions.Average(x => x.Confidence),
            DecisionDistribution = decisionEntries
                .GroupBy(x => x.Timestamp.Date)
                .ToDictionary(x => x.Key, x => x.Count())
        };
    }

    private Task EnqueueAndMaybeFlushAsync(AuditEntry entry)
    {
        _buffer.Enqueue(entry);

        _logger.LogTrace(
            "Audit entry {Id} queued. Buffer size: {Size}.",
            entry.Id,
            _buffer.Count);

        return ShouldFlush() ? FlushAsync() : Task.CompletedTask;
    }

    private bool ShouldFlush()
    {
        return _buffer.Count >= Math.Max(1, _options.BatchSize)
            || (DateTime.UtcNow - _lastFlushTime).TotalSeconds >= _options.FlushIntervalSeconds;
    }

    private AuditEntry CreateEntry(AuditRecordType recordType, Guid correlationId, object data)
    {
        return new AuditEntry
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            SequenceNumber = Interlocked.Increment(ref _sequenceNumber),
            RecordType = recordType,
            CorrelationId = correlationId,
            Data = data
        };
    }
}