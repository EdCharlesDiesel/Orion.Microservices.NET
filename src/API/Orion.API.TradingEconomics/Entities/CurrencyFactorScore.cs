namespace Orion.API.TradingEconomics.Entities
{
    public class CurrencyFactorScore
    {
        public string Currency { get; set; } = default!;
        public DateTime Date { get; set; }
        public decimal Carry { get; set; }
        public decimal Growth { get; set; }
        public decimal Inflation { get; set; }
        public decimal   Risk { get; set; }
        public decimal TotalScore { get; set; }
    }
}
