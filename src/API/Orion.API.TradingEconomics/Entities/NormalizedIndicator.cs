namespace Orion.API.TradingEconomics.Entities
{    

    public class NormalizedIndicator
    {
        public Guid Id { get; set; }

        public string Country { get; set; } = default!;
        public string Indicator { get; set; } = default!;
        public DateTime Date { get; set; }

        public decimal Value { get; set; }
        public decimal? Previous { get; set; }
        public decimal? Forecast { get; set; }

        public decimal YoY { get; set; }
        public decimal MoM { get; set; }
        public decimal ZScore { get; set; }

        public decimal RollingMean { get; set; }
        public decimal RollingStdDev { get; set; }

        public decimal Surprise { get; set; }

        public string Frequency { get; set; } = "Monthly";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Name { get; set; }
    }

    public enum IndicatorFrequency
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly
    }
}
