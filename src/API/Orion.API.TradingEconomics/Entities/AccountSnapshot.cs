namespace Orion.API.TradingEconomics.Entities;

public sealed class AccountSnapshot
{
    public decimal Balance { get; set; }
    public decimal Equity { get; set; }
    public decimal RealizedPnlToday { get; set; }
    public decimal UnrealizedPnl { get; set; }
    public decimal MarginUsed { get; set; }
    public decimal FreeMargin { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}