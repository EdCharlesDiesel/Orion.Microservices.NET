namespace Orion.WebApps.AnalysisDashboard.Models
{
    public class TimeframeConfig
    {
        public string Name { get; set; } = string.Empty;
        public string Interval { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
    }
    

    public static class TimeframeConfigs
    {
        public static readonly Dictionary<string, TimeframeConfig> Mappings = new()
        {
            ["Weekly"] = new() { Name = "Weekly", Interval = "1wk", Period = "3mo" },
            ["Daily"] = new() { Name = "Daily", Interval = "1d", Period = "3mo" },
            ["4 Hour"] = new() { Name = "4 Hour", Interval = "1h", Period = "1mo" },
            ["Hourly"] = new() { Name = "Hourly", Interval = "1h", Period = "1mo" },
            ["15 Minute"] = new() { Name = "15 Minute", Interval = "5m", Period = "5d" }
        };
    }
}
