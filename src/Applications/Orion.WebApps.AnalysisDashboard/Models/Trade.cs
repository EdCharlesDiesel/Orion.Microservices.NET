namespace Orion.WebApps.AnalysisDashboard.Models
{
    public class Trade
    {
        public DateTime DateTime { get; set; }
        public string Direction { get; set; } = string.Empty;
        public decimal Entry { get; set; }
        public string Result { get; set; } = string.Empty;
        public int ProfitAndLoss { get; set; }
        public decimal Exit { get; internal set; }
    }
}