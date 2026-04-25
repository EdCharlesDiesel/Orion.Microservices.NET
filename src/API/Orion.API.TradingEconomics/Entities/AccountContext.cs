namespace Orion.API.TradingEconomics.Entities
{
    public sealed class AccountContext
    {
        public decimal Balance { get; set; }
        public decimal Equity { get; set; }
        public decimal FreeMargin { get; set; }
        public decimal Leverage { get; set; } = 1;
        public string Currency { get; set; } = "USD";
    }
}
