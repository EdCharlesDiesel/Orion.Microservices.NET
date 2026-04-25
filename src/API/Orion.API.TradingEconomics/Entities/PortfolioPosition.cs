namespace Orion.API.TradingEconomics.Entities
{
    public class PortfolioPosition
    {
        public string Pair { get; set; } = default!;
        public string BaseCurrency { get; set; } = default!;
        public string QuoteCurrency { get; set; } = default!;

        public string Direction { get; set; } = default!; // LONG / SHORT

        public decimal SignalStrength { get; set; }
        public decimal Confidence { get; set; }

        public decimal Volatility { get; set; } // e.g. ATR
        public decimal Weight { get; set; }     // normalized portfolio weight
        public decimal PositionSize { get; set; } // capital allocation
    }
}
