namespace Orion.WebApps.AnalysisDashboard.Models
{
    public class TechnicalIndicatorModel
    {
        public double? RSI { get; set; }
        public double? MACD { get; set; }
        public double? MACDSignal { get; set; }
        public double? MACDHistogram { get; set; }
        public decimal? SMA20 { get; set; }
        public decimal? SMA50 { get; set; }
        public decimal? EMA9 { get; set; }
        public decimal? EMA20 { get; set; }
        public decimal? EMA50 { get; set; }
        public decimal? EMA200 { get; set; }
        public decimal? BBUpper { get; set; }
        public decimal? BBMiddle { get; set; }
        public decimal? BBLower { get; set; }
        public decimal? BBWidth { get; set; }
        public decimal? ATR { get; set; }
        public double? StochK { get; set; }
        public double? StochD { get; set; }
        public double? ADX { get; set; }
        public double? ADXPos { get; set; }
        public double? ADXNeg { get; set; }
        public decimal? Resistance20 { get; set; }
        public decimal? Support20 { get; set; }
        public decimal? PivotPoint { get; set; }
    }
}
