namespace Orion.API.TradingEconomics.Entities;

public class ShockResult
{
    public decimal GrowthShock { get; set; }
    public decimal InflationShock { get; set; }
    public decimal SentimentShock { get; set; }
}