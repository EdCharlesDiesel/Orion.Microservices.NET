namespace Orion.API.TradingEconomics.Entities
{
    public sealed class PerformanceReport
    {
        public int TotalTrades { get; set; }
        public int WinningTrades { get; set; }
        public int LosingTrades { get; set; }

        public decimal WinRate { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal GrossLoss { get; set; }
        public decimal NetProfit { get; set; }

        public decimal AverageWin { get; set; }
        public decimal AverageLoss { get; set; }
        public decimal ProfitFactor { get; set; }
        public decimal Expectancy { get; set; }
        public decimal MaxDrawdown { get; set; }
        public decimal AverageRiskReward { get; set; }

        public string Verdict { get; set; } = "";
        public string Reason { get; set; } = "";

        public static PerformanceReport Empty(string reason)
        {
            return new PerformanceReport
            {
                Verdict = "NO_DATA",
                Reason = reason
            };
        }
    }
}
