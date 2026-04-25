namespace Orion.WebApps.AnalysisDashboard.Models
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

}
