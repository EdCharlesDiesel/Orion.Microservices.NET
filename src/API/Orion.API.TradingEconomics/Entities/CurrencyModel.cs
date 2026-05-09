namespace Orion.API.TradingEconomics.Entities
{
    public class CurrencyModel
    {
        public string Currency { get; set; } = default!;

        public decimal CarryWeight { get; set; }
        public decimal GrowthWeight { get; set; }
        public decimal InflationWeight { get; set; }
        public decimal  RiskWeight { get; set; }
    }

    public class FxPrice
    {
        public string Pair { get; set; } = default!;
        public decimal Price { get; set; }
    }

    public class FxReturn
    {
        public string Pair { get; set; } = default!;
        public decimal Return { get; set; }
    }
}
