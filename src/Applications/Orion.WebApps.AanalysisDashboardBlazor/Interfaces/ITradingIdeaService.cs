using Orion.WebApps.AanalysisDashboardBlazor.Models;

namespace Orion.WebApps.AanalysisDashboardBlazor.Interfaces
{
    public interface ITradingIdeaService
    {
        Task<List<TradingIdea>> GenerateTradingIdeasAsync(Dictionary<string, Dictionary<string, List<MarketData>>> dataByTimeframe,MacroDataCollection macro,Dictionary<string, List<MarketData>> dxyByTimeframe);
    }
}
