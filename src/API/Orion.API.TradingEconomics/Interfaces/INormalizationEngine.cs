using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Helpers;

namespace Orion.API.TradingEconomics.Interfaces
{

    public interface INormalizationEngine
    {
        List<NormalizedIndicator> Normalize(IEnumerable<EconomicIndicator> raw);
    }

    
}
