using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Provides FX simulation and pricing utilities.
    /// </summary>
    public interface IFxPricingEngine
    {
        /// <summary>
        /// Runs FX simulation from macro states.
        /// </summary>
        List<FxPrice> Run(List<MacroState> states, Dictionary<string, decimal> initialPrices);

        /// <summary>
        /// Builds pricing metadata for a signal.
        /// </summary>
        PricingResult Price(string signalPair, string signalDirection, decimal positionSize);
    }
}