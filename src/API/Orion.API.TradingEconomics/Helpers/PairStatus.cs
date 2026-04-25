namespace Orion.API.TradingEconomics.Helpers;

public class PairStatus
{
    public string Pair { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsCached { get; set; }
    public TimeSpan? CacheAge { get; set; }
}