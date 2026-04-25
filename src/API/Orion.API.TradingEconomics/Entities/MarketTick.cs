namespace Orion.API.TradingEconomics.Entities
{
    public class MarketTick
    {
        public string Pair { get; set; } = default!;
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public DateTime Time { get; set; }
    }
}
