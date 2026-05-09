namespace Orion.API.TradingEconomics.Entities;

public sealed class PairConfig
{
    public bool Enabled { get; set; } = true;
    public decimal AtrStopMultiplier { get; set; } = 1.5m;
    public decimal MinStopDistance { get; set; } = 0.0010m;
    public decimal MaxSpreadPercent { get; set; } = 0.08m;
    public decimal PipSize { get; set; } = 0.0001m;
}