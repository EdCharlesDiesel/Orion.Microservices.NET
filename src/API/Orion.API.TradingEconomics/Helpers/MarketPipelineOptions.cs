namespace Orion.API.TradingEconomics.Helpers;

public class MarketPipelineOptions
{
    public bool EnableCaching { get; set; } = true;
    public int CacheExpirationSeconds { get; set; } = 300; // 5 minutes
    public int CacheSlidingExpirationSeconds { get; set; } = 60;
    public int ValidationRetries { get; set; } = 2;
    public bool EnableEnrichment { get; set; } = true;
    public bool EnrichWithCorrelations { get; set; } = true;
    public bool EnrichWithSentiment { get; set; } = false;
    public bool EnrichWithMacroCalendar { get; set; } = true;
    public int QuickSnapshotCacheSeconds { get; set; } = 30;
    public int MaxConcurrentPairs { get; set; } = 5;
}