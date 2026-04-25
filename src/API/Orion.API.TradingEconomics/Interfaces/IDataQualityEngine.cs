using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface IDataQualityEngine
    {
        DataQualityResult ValidateCandles(IReadOnlyList<OhlcvBar> candles);
    }
}
