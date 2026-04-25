using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface IIngestionValidator
    {
        bool IsValid(EconomicIndicator indicator);
    }
}
