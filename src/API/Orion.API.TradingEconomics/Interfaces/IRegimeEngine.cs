using Orion.API.TradingEconomics.Enum;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface IRegimeEngine
    {
        MarketRegime Next(MarketRegime current);
    }
}
