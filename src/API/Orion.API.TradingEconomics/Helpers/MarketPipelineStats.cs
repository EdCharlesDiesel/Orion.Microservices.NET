namespace Orion.API.TradingEconomics.Helpers;

public class MarketPipelineStats
{
    public int CachedPairsCount { get; set; }
    public double CacheHitRate { get; set; }
    public double AverageLoadTimeMs { get; set; }
    public long TotalRequests { get; set; }
    public long SuccessfulRequests { get; set; }
    public long FailedRequests { get; set; }
    public long RejectedRequests { get; set; }
        
    public double SuccessRate => TotalRequests > 0 
        ? (double)SuccessfulRequests / TotalRequests * 100 
        : 0;
}