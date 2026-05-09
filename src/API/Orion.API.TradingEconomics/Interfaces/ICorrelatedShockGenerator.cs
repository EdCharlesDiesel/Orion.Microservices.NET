using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    /// <summary>
    /// Interface for correlated shock generator.
    /// </summary>
    public interface ICorrelatedShockGenerator
    {
        ShockResult Generate();
        ShockResult GenerateWithProbabilities(ProbabilisticScenarioResult probabilities);
    }
}