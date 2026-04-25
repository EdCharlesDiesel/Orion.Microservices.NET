namespace Orion.WebApps.AnalysisDashboard.Models
{
    public class SessionAnalysis
    {
        public string Name { get; set; } = string.Empty;
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public double RangePips { get; set; }
        public string Direction { get; set; } = string.Empty;
    }

}
