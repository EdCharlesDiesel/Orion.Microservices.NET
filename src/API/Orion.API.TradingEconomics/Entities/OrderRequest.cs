namespace Orion.API.TradingEconomics.Entities
{
    public sealed class OrderRequest
    {
        public Guid Id { get; set; }

        public string Status { get; set; } = "";
        public string Pair { get; set; } = "";
        public string Direction { get; set; } = "";
        public string OrderType { get; set; } = "MARKET";
        public decimal Quantity { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal StopLoss { get; set; }
        public decimal TakeProfit { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reason { get; set; } = "";

        public static OrderRequest Rejected(string reason)
        {
            return new OrderRequest
            {
                Id = Guid.NewGuid(),
                Status = "REJECTED",
                Reason = reason
            };
        }
    }

    public sealed class OrderState
    {
        public Guid Id { get; set; }

        public string Status { get; set; } = "";
        public string Pair { get; set; } = "";
        public string Direction { get; set; } = "";
        public decimal RequestedQuantity { get; set; }
        public decimal FilledQuantity { get; set; }
        public decimal AverageFillPrice { get; set; }
        public DateTime? FilledAt { get; set; }
        public string Reason { get; set; } = "";

        public static OrderState Rejected(string reason)
        {
            return new OrderState
            {
                Id = Guid.NewGuid(),
                Status = "REJECTED",
                Reason = reason
            };
        }
    }
}
