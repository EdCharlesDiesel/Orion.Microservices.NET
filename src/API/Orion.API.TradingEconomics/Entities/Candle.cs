namespace Orion.API.TradingEconomics.Entities
{
    public class Candle
    {
        public string Pair { get; set; } = default!;
        public DateTime Time { get; set; }

        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }

        public double Volume { get; set; }
    }
}
