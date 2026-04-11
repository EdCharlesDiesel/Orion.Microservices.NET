using Orion.WebApps.AanalysisDashboardBlazor.Models;

namespace Orion.WebApps.AanalysisDashboardBlazor.Interfaces
{
    public interface IMarketDataProvider
    {
        Task<List<MarketData>> GetDataAsync(string symbol, string interval, string period);
    }
}
