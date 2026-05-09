namespace Orion.API.TradingEconomics.Entities
{
    public sealed class MarketQuote
    {
        public string Pair { get; set; } = "";
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Mid => (Bid + Ask) / 2m;
        public decimal Spread => Ask - Bid;
        public DateTime TimestampUtc { get; set; }
        public string Source { get; set; } = "";
    }
}
