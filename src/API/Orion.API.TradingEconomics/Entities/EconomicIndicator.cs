namespace Orion.API.TradingEconomics.Entities
{
    public class EconomicIndicator
    {
        public Guid Id { get; set; }
        public string Country { get; set; } = default!;
        public string Indicator { get; set; } = default!;
        public DateTime Date { get; set; }

        public decimal? Value { get; set; }
        public decimal? Previous { get; set; }
        public decimal? Forecast { get; set; }

        public string Frequency { get; set; } = "Monthly";
        public string Event { get; internal set; }
        public decimal? Actual { get; internal set; }
    }
}
