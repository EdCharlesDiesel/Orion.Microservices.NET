namespace Orion.API.TradingEconomics.Entities;

public sealed class PricingResult
{
    public string Pair { get; set; } = "";
    public string Direction { get; set; } = "";
    public decimal PositionSize { get; set; }
    public string BaseCurrency { get; set; } = "";
    public string QuoteCurrency { get; set; } = "";
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}