namespace Orion.API.TradingEconomics.Entities
{
    /// <summary>
    /// Comprehensive volatility metrics for a trading pair
    /// </summary>
    public class VolatilityMetrics
    {
        public string Pair { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal DailyATR { get; set; }
        public decimal HourlyATR { get; set; }
        public decimal DailyVolatility { get; set; }
        public decimal DailyRange { get; set; }
        public decimal AverageDailyRange { get; set; }
        public string VolatilityRegime { get; set; } = "Unknown";
        public bool IsHighVolatility { get; set; }
        public decimal VolatilityPercentile { get; set; }
        public string? Error { get; set; }

        public decimal VolatilityPercentage => DailyVolatility * 100;
        public decimal ATRAsPercentage => CurrentPrice != 0 ? DailyATR / CurrentPrice * 100 : 0;

        public string Summary =>
            $"{Pair}: ATR={DailyATR:F5} ({ATRAsPercentage:F2}%), " +
            $"Vol={VolatilityPercentage:F1}%, " +
            $"Regime={VolatilityRegime}, " +
            $"Percentile={VolatilityPercentile:F0}";
    }
}
