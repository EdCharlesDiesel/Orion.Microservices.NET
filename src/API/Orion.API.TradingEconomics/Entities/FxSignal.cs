namespace Orion.API.TradingEconomics.Entities
{
    public class FxSignal
    {
        public string BaseCurrency { get; set; } = default!;
        public string QuoteCurrency { get; set; } = default!;
        public string Pair => $"{BaseCurrency}/{QuoteCurrency}";

        public decimal BaseScore { get; set; }
        public decimal QuoteScore { get; set; }

        public decimal SignalStrength { get; set; } // difference
        public string Direction { get; set; } = default!; // LONG / SHORT

        public decimal Confidence { get; set; }
    }
}
