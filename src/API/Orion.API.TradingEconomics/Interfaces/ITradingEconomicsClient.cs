using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface ITradingEconomicsClient
    {
        Task<IEnumerable<EconomicIndicator>> GetIndicatorsAsync(string country);
    }
}
