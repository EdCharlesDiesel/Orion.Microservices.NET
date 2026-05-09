using Orion.WebApps.AnalysisDashboard.Models;

namespace Orion.WebApps.AnalysisDashboard.Interfaces
{
    public interface IMarketDataProvider
    {
        Task<List<MarketData>> GetDataAsync(string symbol, string interval, string period);
    }
}
