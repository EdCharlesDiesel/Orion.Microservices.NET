using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    public interface IFxPricingEngine
    {
        List<FxPrice> Run(List<MacroState> states, Dictionary<string, decimal> initialPrices);

        PricingResult Price(string signalPair, string signalDirection, decimal sizePositionSize);
    }
}