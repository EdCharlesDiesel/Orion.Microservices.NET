using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface ISentimentDataProvider
    {
        string Name { get; }

        Task<IReadOnlyList<SentimentItem>> GetSentimentItemsAsync(
            SentimentDataRequest request,
            CancellationToken cancellationToken = default);
    }
}
