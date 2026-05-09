namespace Orion.API.TradingEconomics.Entities
{
    public sealed class PortfolioRiskResult
    {
        public bool IsAllowed { get; set; }
        public string Reason { get; set; } = "";
        public bool Allowed { get; set; }

        public static PortfolioRiskResult Allow(string reason)
        {
            return new PortfolioRiskResult
            {
                IsAllowed = true,
                Reason = reason
            };
        }

        public static PortfolioRiskResult Block(string reason)
        {
            return new PortfolioRiskResult
            {
                IsAllowed = false,
                Reason = reason
            };
        }
    }
}
