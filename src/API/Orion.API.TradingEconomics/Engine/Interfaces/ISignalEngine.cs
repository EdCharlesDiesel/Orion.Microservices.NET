using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Generates trade signals from market, regime, scenario, probability, and macro data.
    /// </summary>
    public interface ISignalEngine
    {
        /// <summary>
        /// Produces a LONG, SHORT, or NO_TRADE signal.
        /// </summary>
        SignalResult Generate(NormalizedMarketContext market, RegimeResult regime, ScenarioResult scenario, ProbabilisticScenarioResult probabilities, MacroSimulationResult macroSimulation);
    }
}