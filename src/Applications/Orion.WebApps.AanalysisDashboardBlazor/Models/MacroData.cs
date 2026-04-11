namespace Orion.WebApps.AanalysisDashboardBlazor.Models
{
    public class MacroData
    {
        public decimal GDP { get; set; }
        public decimal Inflation { get; set; }
        public decimal Rates { get; set; }
        public decimal Unemployment { get; set; }
    }

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
