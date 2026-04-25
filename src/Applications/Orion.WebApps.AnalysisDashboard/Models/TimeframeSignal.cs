namespace Orion.WebApps.AnalysisDashboard.Models
{
    public class TimeframeSignal
    {
        public string? Bias { get; set; }
        public int Strength { get; set; }
        public List<string> Reasons { get; set; } = new();
    }
}
