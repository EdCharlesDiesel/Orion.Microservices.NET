namespace Orion.WebApps.AnalysisDashboard.Models
{
    public class EnrichedMarketData : MarketData
    {
        public TechnicalIndicatorModel Indicators { get; set; } = new();
    }
}
