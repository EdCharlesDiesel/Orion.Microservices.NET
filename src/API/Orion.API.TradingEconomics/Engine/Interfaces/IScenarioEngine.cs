using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Runs macro shock scenarios and builds scenario impact results.
    /// </summary>
    public interface IScenarioEngine
    {
        /// <summary>
        /// Runs a full scenario simulation.
        /// </summary>
        Task<ScenarioResult> RunAsync(Scenario scenario, CancellationToken cancellationToken = default);

        /// <summary>
        /// Builds a simple scenario result from normalized data and regime.
        /// </summary>
        ScenarioResult Build(NormalizedIndicator normalized, RegimeResult regime);
    }
}