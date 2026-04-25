using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface IMarketDataFeed
    {
        Task<MarketTick> GetLatestTickAsync(string pair, CancellationToken cancellationToken);
    }
    public interface IExecutionCostModel
    {
        decimal EstimateSlippage(string pair, decimal size);
        decimal EstimateSpread(string pair, decimal bid, decimal ask);
    }
}
