
namespace Orion.API.TradingEconomics.Entities
{

    public class TradingIdea
    {
        public string Pair { get; set; } = string.Empty;
        public string Bias { get; set; } = string.Empty;
        public string Conviction { get; set; } = string.Empty;
        public int StrengthScore { get; set; }
        public string Thesis { get; set; } = string.Empty;
        public decimal Entry { get; set; }
        public decimal TakeProfit1 { get; set; }
        public decimal TakeProfit2 { get; set; }
        public string Tp1Method { get; set; } = string.Empty;
        public string Tp2Method { get; set; } = string.Empty;
        public bool Tp1Valid { get; set; }
        public bool Tp2Valid { get; set; }
        public decimal StopLoss { get; set; }
        public string StopLossMethod { get; set; } = string.Empty;
        public decimal StopLossPips { get; set; }
        public decimal RiskReward1 { get; set; }
        public decimal RiskReward2 { get; set; }
        public decimal ATR { get; set; }
        public EntrySignalResult? EntrySignal { get; set; }
    }

    public class SwingTradingIdea : TradingIdea
    {
        public string Invalidation { get; set; } = string.Empty;
        public string WeeklyTrend { get; set; } = string.Empty;
        public string DailyTrend { get; set; } = string.Empty;
        public bool H4Confirmation { get; set; }
    }

    public class BiasAnalysisResult
    {
        public string Pair { get; set; } = string.Empty;
        public string OverallBias { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public Dictionary<string, TimeframeBiasData> Timeframes { get; set; } = new();
        public DateTime Timestamp { get; internal set; }
        public bool IsTradeable { get; internal set; }
        public string Error { get; internal set; }
        public object TimeframeBiases { get; internal set; }
    }

    public class TimeframeBiasData
    {
        public string Bias { get; set; } = string.Empty;
        public int Strength { get; set; }
        public decimal Price { get; set; }
        public string Trend { get; set; } = string.Empty;
        public decimal RSI { get; set; }
        public decimal ADX { get; set; }
    }
}
