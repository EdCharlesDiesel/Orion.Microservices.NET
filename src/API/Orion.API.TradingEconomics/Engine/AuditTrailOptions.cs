namespace Orion.API.TradingEconomics.Engine;

public class AuditTrailOptions
{
    public int BatchSize { get; set; } = 100;
    public int FlushIntervalSeconds { get; set; } = 30;
    public bool Enabled { get; set; } = true;
    public bool DetailedLogging { get; set; } = false;
    public List<string> ExcludedSteps { get; set; } = new();
    public int MaxRetryAttempts { get; set; } = 3;
    public string StorageType { get; set; } = "File"; // File, Database, Elasticsearch, Cloud
}