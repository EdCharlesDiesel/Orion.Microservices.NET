namespace Orion.API.TradingEconomics.Entities
{
    public sealed class PositionSizeResult
    {
        public bool IsAllowed { get; set; }
        public string Pair { get; set; } = "";
        public string Direction { get; set; } = "";
        public decimal PositionSize { get; set; }
        public decimal RiskAmount { get; set; }
        public decimal RiskPercent { get; set; }
        public decimal StopDistance { get; set; }
        public string Reason { get; set; } = "";

        public static PositionSizeResult None(string reason)
        {
            return new PositionSizeResult
            {
                IsAllowed = false,
                Direction = "NO_TRADE",
                Reason = reason
            };
        }
    }
}
