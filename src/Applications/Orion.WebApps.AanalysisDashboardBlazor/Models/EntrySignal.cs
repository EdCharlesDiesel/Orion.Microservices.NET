namespace Orion.WebApps.AanalysisDashboardBlazor.Models
{
    public class EntrySignal
    {
        public int Signal { get; set; } // -1: Short, 0: Neutral, 1: Long
        public int Confidence { get; set; }
        public List<string> Reasons { get; set; } = new();
        public double StochK { get; set; }
        public double StochD { get; set; }
        public double RSI { get; set; }
        public decimal Price { get; set; }
    }

    public class TimeframeSignal
    {
        public string? Bias { get; set; }
        public int Strength { get; set; }
        public List<string> Reasons { get; set; } = new();
    }

    public class TradingIdeaSignal 
    {
        public string Pair { get; set; } = string.Empty;
        public string Bias { get; set; } = string.Empty;
        public string Conviction { get; set; } = string.Empty;
        public int StrengthScore { get; set; }
        public string Thesis { get; set; } = string.Empty;
        public decimal Entry { get; set; }
        public decimal TakeProfit1 { get; set; }
        public decimal TakeProfit2 { get; set; }
        public decimal StopLoss { get; set; }
        public decimal RiskReward { get; set; }
        public EntrySignal? EntrySignal { get; set; }
        public Dictionary<string, TimeframeSignal> TimeframeSignals { get; set; } = new();
    }

    public class PerformanceData
    {
        public string Asset { get; set; } = string.Empty;
        public decimal ReturnPercent { get; set; }
    }
}
