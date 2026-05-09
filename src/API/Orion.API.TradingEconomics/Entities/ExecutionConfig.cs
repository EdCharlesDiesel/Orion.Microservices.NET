namespace Orion.API.TradingEconomics.Entities;

public sealed class ExecutionConfig
{
    public decimal MaxSlippagePercent { get; set; } = 0.10m;
    public decimal MinFillRatio { get; set; } = 0.95m;
    public bool UseAdvancedExecution { get; set; } = true;
}