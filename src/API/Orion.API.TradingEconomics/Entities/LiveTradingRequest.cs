namespace Orion.API.TradingEconomics.Entities
{
    public sealed class LiveTradingRequest
    {
        public ForexMarketInput MarketInput { get; set; } = new();
        public AccountContext Account { get; set; } = new();
        public OrderBook OrderBook { get; set; } = new();
    }
}
