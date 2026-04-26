using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Interface for macro simulation engine.
    /// </summary>
    public interface IMacroSimulationEngine
    {
        /// <summary>
        /// Runs a macro state simulation for specified number of steps.
        /// </summary>
        List<MacroState> Run(MacroState initial, int steps);

        /// <summary>
        /// Simulates macro outcomes based on normalized indicators and probabilities.
        /// </summary>
        MacroSimulationResult Simulate(NormalizedIndicator normalized, RegimeResult regime, ProbabilisticScenarioResult probabilities);
    }
}