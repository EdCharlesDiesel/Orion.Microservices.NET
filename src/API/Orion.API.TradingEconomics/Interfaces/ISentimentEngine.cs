using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface ISentimentEngine
    {       

        public Task<SentimentResult> AnalyzeAsync(SentimentRequest request, CancellationToken cancellationToken = default);
    }
}
