namespace Orion.API.TradingEconomics.Entities
{
    /// <summary>
    /// Spread response from API
    /// </summary>
    public class SpreadResponse
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Spread { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
