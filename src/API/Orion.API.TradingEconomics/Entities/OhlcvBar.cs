namespace Orion.API.TradingEconomics.Entities
{
    public sealed class OhlcvBar
    {
        public string Pair { get; set; } = "";
        public DateTime TimestampUtc { get; set; }

        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }

        public string Timeframe { get; set; } = "";
        public string Source { get; set; } = "";
    }
}
