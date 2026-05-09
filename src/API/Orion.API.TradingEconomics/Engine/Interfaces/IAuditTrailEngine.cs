using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces;

public interface IAuditTrailEngine
{
    /// <summary>
    /// Records a full trading decision audit record.
    /// </summary>
    Task<Guid> RecordDecisionAsync(AuditRecord record);

    /// <summary>
    /// Records a single pipeline step.
    /// </summary>
    public Task RecordPipelineStepAsync<T>(Guid correlationId, string stepName, T stepData, TimeSpan duration);

    /// <summary>
    /// Records an execution error.
    /// </summary>
    Task RecordErrorAsync(Guid correlationId, string stage, Exception exception, Dictionary<string, object>? context = null);

    /// <summary>
    /// Records a business or system event.
    /// </summary>
    Task RecordEventAsync(Guid correlationId, string eventName, Dictionary<string, object>? metadata = null);

    /// <summary>
    /// Flushes buffered audit records to storage.
    /// </summary>
    Task FlushAsync();

    /// <summary>
    /// Queries audit records from storage.
    /// </summary>
    Task<AuditQueryResult> QueryAsync(AuditQuery query);

    /// <summary>
    /// Generates a compliance report from stored decision audit records.
    /// </summary>
    Task<ComplianceReport> GenerateComplianceReportAsync(DateTime startDate, DateTime endDate, string? pair = null);
}