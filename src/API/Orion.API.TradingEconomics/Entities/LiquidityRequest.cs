namespace Orion.API.TradingEconomics.Entities
{
    public sealed class LiquidityRequest
    {
        public string Pair { get; set; } = "";
        public List<OrderBookLevel> Bids { get; set; } = new();
        public List<OrderBookLevel> Asks { get; set; } = new();
        public decimal RequestedSize { get; set; }
    }

    public sealed class OrderBookLevel
    {
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
    }

    public sealed class LiquidityResult
    {
        public string Pair { get; set; } = "";
        public decimal BestBid { get; set; }
        public decimal BestAsk { get; set; }
        public decimal Spread { get; set; }
        public decimal SpreadPercent { get; set; }
        public decimal BidDepth { get; set; }
        public decimal AskDepth { get; set; }
        public decimal TotalDepth { get; set; }
        public decimal Imbalance { get; set; }
        public decimal EstimatedSlippagePercent { get; set; }
        public string LiquidityGrade { get; set; } = "";
        public string RiskSummary { get; set; } = "";
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    }
}
