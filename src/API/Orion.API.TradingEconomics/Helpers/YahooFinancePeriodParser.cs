namespace Orion.API.TradingEconomics.Helpers
{
    public static class YahooFinancePeriodParser
    {
        private static readonly Dictionary<string, (string Range, string Interval)> PeriodMap = new()
        {
            ["1d"] = ("1d", "5m"),
            ["5d"] = ("5d", "15m"),
            ["1mo"] = ("1mo", "1h"),
            ["3mo"] = ("3mo", "1d"),
            ["6mo"] = ("6mo", "1d"),
            ["1y"] = ("1y", "1d"),
            ["2y"] = ("2y", "1wk"),
            ["5y"] = ("5y", "1mo")
        };

        public static TimeSpan ParseToTimeSpan(string period) => period switch
        {
            "1d" => TimeSpan.FromDays(1),
            "5d" => TimeSpan.FromDays(5),
            "1mo" => TimeSpan.FromDays(30),
            "3mo" => TimeSpan.FromDays(90),
            "6mo" => TimeSpan.FromDays(180),
            "1y" => TimeSpan.FromDays(365),
            _ => TimeSpan.FromDays(30)
        };

        public static string ParseToYahooRange(string period) => period switch
        {
            "1d" => "1d",
            "5d" => "5d",
            "1mo" => "1mo",
            "3mo" => "3mo",
            "6mo" => "6mo",
            "1y" => "1y",
            "2y" => "2y",
            "5y" => "5y",
            _ => "1mo"
        };

        public static string ParseToYahooInterval(string interval) => interval switch
        {
            "1m" => "1m",
            "5m" => "5m",
            "15m" => "15m",
            "30m" => "30m",
            "1h" => "1h",
            "1d" => "1d",
            "1wk" => "1wk",
            "1mo" => "1mo",
            // Custom mapping for unsupported intervals
            "4h" => "1h", // Fallback - you'll need to aggregate later
            _ => "1d"
        };

        public static (string Range, string Interval) GetYahooParameters(string period, string interval)
        {
            var range = ParseToYahooRange(period);
            var yahooInterval = ParseToYahooInterval(interval);

            // Auto-adjust interval based on range if not explicitly set
            if (yahooInterval == "1h" && (range == "3mo" || range == "6mo"))
            {
                yahooInterval = "1d"; // Yahoo limits 1h data to 730 days
            }

            return (range, yahooInterval);
        }
    }
}
