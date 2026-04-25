namespace Orion.API.TradingEconomics.Entities
{
    public sealed class ExitPlan
    {
        public decimal StopLoss { get; set; }
        public decimal TakeProfit { get; set; }
        public decimal RiskRewardRatio { get; set; }
        public string Reason { get; set; } = "";
        public string Pair { get; set; }
        public string Direction { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal TrailingStopDistance { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
