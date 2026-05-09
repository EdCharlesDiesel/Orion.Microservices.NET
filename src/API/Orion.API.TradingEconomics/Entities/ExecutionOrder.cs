namespace Orion.API.TradingEconomics.Entities
{
    public class ExecutionOrder
    {
        public string Pair { get; set; } = default!;
        public string Direction { get; set; } = default!;

        public decimal RequestedSize { get; set; }
        public decimal FilledSize { get; set; }

        public decimal RequestedPrice { get; set; }
        public decimal ExecutedPrice { get; set; }
        public decimal SpreadCost { get; set; }
        public decimal SlippageCost { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
