namespace Orion.API.TradingEconomics.Entities
{
    public class TechnicalIndicators
    {
        public List<decimal?> RSI { get; set; } = new();
        public List<decimal?> MACD { get; set; } = new();
        public List<decimal?> MACDSignal { get; set; } = new();
        public List<decimal?> MACDHistogram { get; set; } = new();
        public List<decimal?> SMA20 { get; set; } = new();
        public List<decimal?> SMA50 { get; set; } = new();
        public List<decimal?> EMA20 { get; set; } = new();
        public List<decimal?> EMA50 { get; set; } = new();
        public List<decimal?> BBUpper { get; set; } = new();
        public List<decimal?> BBMiddle { get; set; } = new();
        public List<decimal?> BBLower { get; set; } = new();
        public List<decimal?> ATR { get; set; } = new();
        public List<decimal?> StochK { get; set; } = new();
        public List<decimal?> StochD { get; set; } = new();
        public List<decimal?> ADX { get; set; } = new();
        public List<decimal?> ADXPos { get; set; } = new();
        public List<decimal?> ADXNeg { get; set; } = new();
        public List<decimal?> Support20 { get; set; } = new();
        public List<decimal?> Resistance20 { get; set; } = new();
    }

    public class EntrySignalResult
    {
        public int Signal { get; set; }
        public int Confidence { get; set; }
        public List<string> Reasons { get; set; } = new();
        public decimal StochK { get; set; }
        public decimal StochD { get; set; }
        public decimal RSI { get; set; }
        public decimal Price { get; set; }
    }
}
