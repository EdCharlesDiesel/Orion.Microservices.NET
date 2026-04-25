using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class AuditTrailEngine
    {
        private readonly ILogger<AuditTrailEngine> _logger;
        private readonly AuditTrailOptions _options;
        private readonly ConcurrentQueue<AuditEntry> _buffer;
        private readonly SemaphoreSlim _flushLock;
        private readonly IAuditStorage _storage;
        private DateTime _lastFlushTime;
        private int _sequenceNumber;

        public AuditTrailEngine(
            ILogger<AuditTrailEngine> logger,
            IOptions<AuditTrailOptions> options,
            IAuditStorage storage)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? new AuditTrailOptions();
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _buffer = new ConcurrentQueue<AuditEntry>();
            _flushLock = new SemaphoreSlim(1, 1);
            _lastFlushTime = DateTime.UtcNow;
            _sequenceNumber = 0;
        }

        /// <summary>
        /// Records a complete pipeline execution with all decision factors
        /// </summary>
        public async Task<Guid> RecordDecisionAsync(AuditRecord record)
        {
            var entry = new AuditEntry
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                SequenceNumber = Interlocked.Increment(ref _sequenceNumber),
                RecordType = AuditRecordType.Decision,
                Data = record,
                CorrelationId = record.CorrelationId,
                SessionId = record.SessionId,
                Pair = record.Input?.Pair,
                Direction = record.Decision?.Direction,
                Confidence = record.Decision?.Confidence
            };

            await EnqueueAndMaybeFlushAsync(entry);
            return entry.Id;
        }

        /// <summary>
        /// Records a specific step in the pipeline
        /// </summary>
        public async Task RecordPipelineStepAsync<T>(Guid correlationId, string stepName, T stepData, TimeSpan duration)
        {
            var entry = new AuditEntry
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                SequenceNumber = Interlocked.Increment(ref _sequenceNumber),
                RecordType = AuditRecordType.PipelineStep,
                CorrelationId = correlationId,
                Data = new PipelineStepAudit
                {
                    StepName = stepName,
                    Data = stepData,
                    Duration = duration,
                    DataType = typeof(T).Name
                }
            };

            await EnqueueAndMaybeFlushAsync(entry);
        }

        /// <summary>
        /// Records errors or warnings during execution
        /// </summary>
        public async Task RecordErrorAsync(Guid correlationId, string stage, Exception exception, Dictionary<string, object> context = null)
        {
            var entry = new AuditEntry
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                SequenceNumber = Interlocked.Increment(ref _sequenceNumber),
                RecordType = AuditRecordType.Error,
                CorrelationId = correlationId,
                Data = new ErrorAudit
                {
                    Stage = stage,
                    ExceptionType = exception.GetType().Name,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    Context = context ?? new Dictionary<string, object>()
                }
            };

            await EnqueueAndMaybeFlushAsync(entry);
        }

        /// <summary>
        /// Records state changes or important events
        /// </summary>
        public async Task RecordEventAsync(Guid correlationId, string eventName, Dictionary<string, object> metadata = null)
        {
            var entry = new AuditEntry
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                SequenceNumber = Interlocked.Increment(ref _sequenceNumber),
                RecordType = AuditRecordType.Event,
                CorrelationId = correlationId,
                Data = new EventAudit
                {
                    EventName = eventName,
                    Metadata = metadata ?? new Dictionary<string, object>()
                }
            };

            await EnqueueAndMaybeFlushAsync(entry);
        }

        private async Task EnqueueAndMaybeFlushAsync(AuditEntry entry)
        {
            _buffer.Enqueue(entry);

            _logger.LogTrace("Audit entry {Id} queued. Buffer size: {Size}", 
                entry.Id, _buffer.Count);

            if (ShouldFlush())
            {
                await FlushAsync();
            }
        }

        private bool ShouldFlush()
        {
            return _buffer.Count >= _options.BatchSize ||
                   (DateTime.UtcNow - _lastFlushTime).TotalSeconds >= _options.FlushIntervalSeconds;
        }

        /// <summary>
        /// Force flush all buffered entries to storage
        /// </summary>
        public async Task FlushAsync()
        {
            await _flushLock.WaitAsync();
            try
            {
                if (_buffer.IsEmpty)
                    return;

                var entries = new List<AuditEntry>();
                while (_buffer.TryDequeue(out var entry))
                {
                    entries.Add(entry);
                }

                if (entries.Count > 0)
                {
                    await _storage.StoreBatchAsync(entries);
                    _lastFlushTime = DateTime.UtcNow;
                    
                    _logger.LogDebug(
                        "Flushed {Count} audit entries to storage", entries.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to flush audit entries");
                // Re-queue entries that failed to save
                // Consider a dead-letter queue for persistent failures
            }
            finally
            {
                _flushLock.Release();
            }
        }

        /// <summary>
        /// Query audit records with filters
        /// </summary>
        public async Task<AuditQueryResult> QueryAsync(AuditQuery query)
        {
            return await _storage.QueryAsync(query);
        }

        /// <summary>
        /// Generate compliance report
        /// </summary>
        public async Task<ComplianceReport> GenerateComplianceReportAsync(DateTime startDate, DateTime endDate, string pair = null)
        {
            var query = new AuditQuery
            {
                StartDate = startDate,
                EndDate = endDate,
                Pair = pair,
                RecordType = AuditRecordType.Decision
            };

            var result = await _storage.QueryAsync(query);

            return new ComplianceReport
            {
                GeneratedAt = DateTime.UtcNow,
                Period = new DateRange(startDate, endDate),
                Pair = pair,
                TotalDecisions = result.TotalCount,
                TradesByDirection = result.Entries
                    .OfType<TradingDecision>()
                    .GroupBy(d => d.Direction)
                    .ToDictionary(g => g.Key, g => g.Count()),
                AverageConfidence = result.Entries
                    .OfType<TradingDecision>()
                    .Average(d => d.Confidence),
                DecisionDistribution = result.Entries
                    .GroupBy(e => e.Timestamp.Date)
                    .ToDictionary(g => g.Key, g => g.Count())
            };
        }
    }

}