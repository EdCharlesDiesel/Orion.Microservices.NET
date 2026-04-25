namespace Orion.API.TradingEconomics.Entities;

public sealed class RiskConfig
{
    public decimal MaximumSpreadPercent { get; set; } = 0.08m;
    public decimal MaximumVolatilityPercent { get; set; } = 2.5m;
    public decimal MaximumTotalRiskScore { get; set; } = 0.75m;

    public decimal DefaultRiskPerTradePercent { get; set; } = 1.0m;
    public decimal MaxRiskPerTradePercent { get; set; } = 2.0m;
    public decimal MinRiskRewardRatio { get; set; } = 1.5m;
}