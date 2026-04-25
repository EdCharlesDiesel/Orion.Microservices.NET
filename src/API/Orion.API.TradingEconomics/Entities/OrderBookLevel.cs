namespace Orion.API.TradingEconomics.Entities
{

    public class OrderBook
    {
        public string Pair { get; set; } = default!;
        public List<OrderBookLevel> Bids { get; set; } = new();
        public List<OrderBookLevel> Asks { get; set; } = new();
        public DateTime Timestamp { get; set; }
    }

    public class ExecutionResult
    {
        public ExecutionOrder Order { get; set; } = default!;
        public bool PartialFill { get; set; }
        public decimal FillRatio { get; set; }
        public decimal LatencyMs { get; set; }
        public bool HighImpactEvent { get; set; }
    }
}
