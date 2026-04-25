using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface IYahooFinanceService
    {
        Task<List<MarketDataResponse>> FetchDataAsync(string pair,string interval,string period,CancellationToken cancellationToken = default);
        Task<Dictionary<string, MarketDataResponse>> FetchAllTimeframesAsync(string pair,CancellationToken cancellationToken = default);
        Task<List<KpiData>> GetKpisAsync(CancellationToken cancellationToken = default);
    }    
}
