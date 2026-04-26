using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    public interface INormalizationEngine
    {
        List<NormalizedIndicator> Normalize(IEnumerable<EconomicIndicator> raw);
    }
}
