namespace Orion.API.TradingEconomics.Entities
{
    
    public class AssetPair
    {
        public string DisplayName { get; set; } = string.Empty;
        public string YahooSymbol { get; set; } = string.Empty;
    }

    public class TimeframeConfig
    {
        public string Interval { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
    }

    public class MarketDataResponse
    {
        public string Pair { get; set; } = string.Empty;
        public string Timeframe { get; set; } = string.Empty;
        public List<OhlcvBar> OhlcvBar { get; set; } = new();
        public Dictionary<string, List<decimal?>> Indicators { get; set; } = new();
        public DateTime LastUpdated { get; set; }
        
    }

    public class KpiData
    {
        public string Pair { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal ChangePercent { get; set; }
    }
}
