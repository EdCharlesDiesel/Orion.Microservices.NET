namespace Orion.WebApps.AnalysisDashboard.Models
{
    public class MacroDataCollection
    {
        public MacroData USD { get; set; } = new();
        public MacroData ZAR { get; set; } = new();
        public MacroData JPY { get; set; } = new();
        public MacroData AUD { get; set; } = new();
        public MacroData NZD { get; set; } = new();
        public MacroData CAD { get; set; } = new();
        public MacroData EUR { get; set; } = new();
        public MacroData GBP { get; set; } = new();
        public MacroData CHF { get; set; } = new();
    }
}
