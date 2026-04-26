namespace Orion.API.TradingEconomics.Entities
{
    public sealed class SignalResult
    {
        public string Pair { get; set; } = "";
        public string Direction { get; set; } = "NO_TRADE";
        public decimal Confidence { get; set; }
        public decimal Score { get; set; }
        public string Reason { get; set; } = "";

        public static SignalResult NoTrade(string reason)
        {
            return new SignalResult
            {
                Direction = "NO_TRADE",
                Confidence = 0,
                Score = 0,
                Reason = reason
            };
        }
    }

    public sealed class NormalizedMarketContext
    {
        public string Pair { get; set; } = "";
        public List<OhlcvBar> Candles { get; set; } = new();
        public decimal Spread { get; set; }
    }

    public sealed class RegimeResult
    {
        public string Name { get; set; } = "NEUTRAL";
        public MarketRegime CurrentRegime { get; set; }
        public MarketRegime Regime { get; set; }
        public decimal Confidence { get; set; }
        public string Reason { get; set; }
        public DateTime TimestampUtc { get; set; }
    }
    public sealed class ProbabilisticScenarioResult
    {
        public decimal Probability { get; set; }
        public int ScenarioCount { get; set; }
    }

    public sealed class MacroSimulationResult
    {
        public string Direction { get; set; } = "NEUTRAL";
        public decimal Confidence { get; set; }
        public List<MacroState> States { get; set; }
        public MarketRegime FinalRegime { get; set; }
        public double SuccessRate { get; set; }
        public DateTime TimestampUtc { get; set; }
    }

}
