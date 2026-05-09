namespace Orion.API.TradingEconomics.Entities;

public sealed class LiveTradingConfig
{
    public bool Enabled { get; set; } = false;
    public bool PaperTradingOnly { get; set; } = true;
    public int MaxOpenTrades { get; set; } = 3;
    public decimal MaxDailyLossPercent { get; set; } = 3m;
}