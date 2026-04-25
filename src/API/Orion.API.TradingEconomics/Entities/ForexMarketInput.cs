namespace Orion.API.TradingEconomics.Entities
{
    public sealed class ForexMarketInput
    {
        public string Pair { get; set; } = "";
        public List<OhlcvBar> Candles { get; set; } = new();
        public List<MacroEvent> MacroEvents { get; set; } = new();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string DataSource { get; set; }
        public object CorrelatedData { get; set; }
        public DateTime LoadedAt { get; set; }
    }

    public sealed class TradingDecision
    {
        public string Pair { get; set; } = "";
        public string Direction { get; set; } = "";
        public decimal Confidence { get; set; }
        public string Regime { get; set; } = "";
        public string Scenario { get; set; } = "";
        public decimal RiskScore { get; set; }
        public decimal PositionSize { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal StopLoss { get; set; }
        public decimal TakeProfit { get; set; }
        public string Reason { get; set; } = "";

        public static TradingDecision NoTrade(string reason)
        {
            return new TradingDecision
            {
                Direction = "NO_TRADE",
                Reason = reason
            };
        }
    }


}
