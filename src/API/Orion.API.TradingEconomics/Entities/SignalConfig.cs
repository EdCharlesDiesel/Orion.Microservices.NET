namespace Orion.API.TradingEconomics.Entities;

public sealed class SignalConfig
{
    public decimal MinimumConfidence { get; set; } = 55;

    public decimal TechnicalWeight { get; set; } = 0.35m;
    public decimal RegimeWeight { get; set; } = 0.25m;
    public decimal ScenarioWeight { get; set; } = 0.25m;
    public decimal MacroWeight { get; set; } = 0.15m;
}