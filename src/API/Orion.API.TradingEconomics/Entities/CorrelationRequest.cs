namespace Orion.API.TradingEconomics.Entities
{
    public sealed class CorrelationRequest
    {
        public string PrimaryPair { get; set; } = "";
        public List<CorrelationSeries> Series { get; set; } = new();
        public int LookbackPeriods { get; set; } = 50;
    }

    public sealed class CorrelationSeries
    {
        public string Symbol { get; set; } = "";
        public List<decimal> Values { get; set; } = new();
    }

    public sealed class CorrelationResult
    {
        public string PrimaryPair { get; set; } = "";
        public List<CorrelationItem> Correlations { get; set; } = new();
        public string RiskSummary { get; set; } = "";
        public decimal AverageAbsCorrelation { get; set; }
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    }

    public sealed class CorrelationItem
    {
        public string Symbol { get; set; } = "";
        public decimal Correlation { get; set; }
        public string Strength { get; set; } = "";
        public string Direction { get; set; } = "";
    }
}
