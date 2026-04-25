namespace Orion.API.TradingEconomics.Entities
{
    public class MacroState
    {
        public decimal Inflation { get; set; }
        public decimal InterestRate { get; set; }
        public decimal Growth { get; set; }
        public decimal   RiskSentiment { get; set; } // -1 to +1

        public Dictionary<string, decimal> CurrencyStrength { get; set; } = new();
    }


    public enum MarketRegime
    {
        RiskOn,
        RiskOff,
        Stagflation,
        Goldilocks
    }
}
