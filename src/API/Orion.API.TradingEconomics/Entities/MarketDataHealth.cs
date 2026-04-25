namespace Orion.API.TradingEconomics.Entities
{
    public sealed class MarketDataHealth
    {
        public string Pair { get; set; } = "";
        public bool IsHealthy { get; set; }
        public bool IsStale { get; set; }
        public DateTime? LastTimestampUtc { get; set; }
        public List<string> Issues { get; set; } = new();
    }
}
