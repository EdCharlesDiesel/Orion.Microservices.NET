namespace Orion.API.TradingEconomics.Entities
{
    public sealed class TradePlan
    {
        public Guid Id { get; set; }

        public string Status { get; set; } = "REJECTED";
        public string Pair { get; set; } = "";
        public string Direction { get; set; } = "";
        public decimal EntryPrice { get; set; }
        public decimal PositionSize { get; set; }
        public decimal StopLoss { get; set; }
        public decimal TakeProfit { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal ProfitLoss { get; set; }
        public DateTime OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string CloseReason { get; set; } = "";
        public string Reason { get; set; } = "";
        public decimal ExitPrice { get; set; }

        public static TradePlan Rejected(string reason)
        {
            return new TradePlan
            {
                Id = Guid.NewGuid(),
                Status = "REJECTED",
                Reason = reason
            };
        }

        public TradePlan Close(string reason, decimal closePrice, DateTime closedAt)
        {
            Status = "CLOSED";
            CloseReason = reason;
            ClosePrice = closePrice;
            ClosedAt = closedAt;

            ProfitLoss = Direction == "LONG"
                ? (closePrice - EntryPrice) * PositionSize
                : (EntryPrice - closePrice) * PositionSize;

            return this;
        }
    }
}
