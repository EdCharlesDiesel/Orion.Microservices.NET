namespace Orion.API.TradingEconomics.Engine;

public class AuditQueryResult
{
    public List<AuditEntry> Entries { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public bool HasMorePages { get; set; }
}