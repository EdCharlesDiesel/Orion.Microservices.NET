using Orion.API.TradingEconomics.Enum;
namespace Orion.API.TradingEconomics.Entities;

public class AuditQuery
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Pair { get; set; }
    public string Direction { get; set; }
    public AuditRecordType? RecordType { get; set; }
    public Guid? CorrelationId { get; set; }
    public decimal? MinConfidence { get; set; }
    public decimal? MaxConfidence { get; set; }
    public int PageSize { get; set; } = 100;
    public int PageNumber { get; set; } = 1;
    public string SortBy { get; set; } = "Timestamp";
    public bool SortDescending { get; set; } = true;
    public int Page { get; set; }
}