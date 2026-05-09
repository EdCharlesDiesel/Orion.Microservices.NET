namespace Orion.API.TradingEconomics.Entities
{
    public class MacroState
    {
        public decimal Inflation { get; set; }
        public decimal InterestRate { get; set; }
        public decimal Growth { get; set; }
        public decimal   RiskSentiment { get; set; } // -1 to +1

        public Dictionary<string, decimal> CurrencyStrength { get; set; } = new();
        public decimal GdpGrowth { get; set; }
        public decimal Sentiment { get; set; }
        public bool IsStable { get; set; }
        public DateTime TimestampUtc { get; set; }
    }
}
