using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    public interface IDataQualityEngine
    {
        DataQualityResult ValidateCandles(IReadOnlyList<OhlcvBar> candles);
    }
}
