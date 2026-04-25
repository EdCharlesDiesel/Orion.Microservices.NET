namespace Orion.API.TradingEconomics.Entities
{
    public sealed class MarketDataRequest
    {
        public string Pair { get; set; } = "";
        public string Timeframe { get; set; } = "1h";
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public string Provider { get; set; } = "Yahoo";
    }
}
