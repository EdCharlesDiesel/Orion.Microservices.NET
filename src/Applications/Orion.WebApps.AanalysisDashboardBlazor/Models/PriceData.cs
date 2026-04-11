namespace Orion.WebApps.AanalysisDashboardBlazor.Models
{
    public class PriceData
    {
        public DateTime DateTime { get; set; }
        public DateOnly Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }

        // Indicators
        public decimal? Ema9 { get; set; }
        public decimal? Ema21 { get; set; }
        public decimal? Ema50 { get; set; }
        public double? Rsi { get; set; }
        public decimal? Macd { get; set; }
        public decimal? Signal { get; set; }
        public decimal? BbUpper { get; set; }
        public decimal? BbLower { get; set; }
        public decimal? BbMid { get; set; }
    }

    public class Trade
    {
        public DateTime DateTime { get; set; }
        public string Date { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty;
        public decimal Entry { get; set; }
        public string Result { get; set; } = string.Empty;
        public int PnL { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal ExitPrice { get; set; }
    }

    public class SessionAnalysis
    {
        public string Name { get; set; } = string.Empty;
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public double RangePips { get; set; }
        public string Direction { get; set; } = string.Empty;
    }

    public class TradingIdea
    {
        public string Type { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }

    public class KeyLevels
    {
        public decimal Pp { get; set; }
        public decimal R1 { get; set; }
        public decimal R2 { get; set; }
        public decimal S1 { get; set; }
        public decimal S2 { get; set; }
    }

}
