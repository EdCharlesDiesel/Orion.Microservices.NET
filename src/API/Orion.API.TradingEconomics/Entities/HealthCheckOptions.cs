namespace Orion.API.TradingEconomics.Entities;

public class HealthCheckOptions
{
    public int CheckIntervalSeconds { get; set; } = 30;
    public int InitialDelaySeconds { get; set; } = 5;
    public int MaxHistoryItems { get; set; } = 1000;
    public double MaxMemoryThresholdMB { get; set; } = 2048;
    public double MaxCpuPercent { get; set; } = 80;
    public bool DetailedMetricsEnabled { get; set; } = true;
    public List<string> ExcludedComponents { get; set; } = new();
    public Dictionary<string, object> CustomThresholds { get; set; } = new();
}