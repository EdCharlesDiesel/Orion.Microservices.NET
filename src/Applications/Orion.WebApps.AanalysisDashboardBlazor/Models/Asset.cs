namespace Orion.WebApps.AanalysisDashboardBlazor.Models
{
    public class Asset
    {
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
    }

    public static class Assets
    {
        public static readonly Dictionary<string, string> All = new()
    {
        { "EUR/USD", "EURUSD=X" },
        { "GBP/USD", "GBPUSD=X" },
        { "USD/JPY", "JPY=X" },
        { "USD/ZAR", "USDZAR=X" },
        { "AUD/USD", "AUDUSD=X" },
        { "NZD/USD", "NZDUSD=X" },
        { "USD/CAD", "CAD=X" },
        { "USD/CHF", "CHF=X" },
        { "XAU/USD", "XAUUSD=X" }
    };
    }
}
