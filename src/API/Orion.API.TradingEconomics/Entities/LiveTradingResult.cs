namespace Orion.API.TradingEconomics.Entities
{
    public sealed class LiveTradingResult
    {
        public string Status { get; set; } = "";
        public string Pair { get; set; } = "";
        public string Direction { get; set; } = "";
        public decimal Confidence { get; set; }
        public string Regime { get; set; } = "";
        public string Scenario { get; set; } = "";
        public decimal PositionSize { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal StopLoss { get; set; }
        public decimal TakeProfit { get; set; }
        public decimal RiskScore { get; set; }
        public string Reason { get; set; } = "";
        public TradePlan? Trade { get; set; }

        public static LiveTradingResult Blocked(string status, string reason)
        {
            return new LiveTradingResult
            {
                Status = status,
                Direction = "NO_TRADE",
                Reason = reason
            };
        }
    }
}
