using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Helpers
{
    /// <summary>
    /// Placeholder implementations for dependencies.
    /// </summary>


    public class CorrelatedShockGenerator : ICorrelatedShockGenerator
    {
        private static readonly Random Random = new();

        public ShockResult Generate() => new()
        {
            GrowthShock = (decimal)(Random.NextDouble() * 0.2 - 0.1),
            InflationShock = (decimal)(Random.NextDouble() * 0.15 - 0.075),
            SentimentShock = (decimal)(Random.NextDouble() * 0.3 - 0.15)
        };

        public ShockResult GenerateWithProbabilities(ProbabilisticScenarioResult probabilities) => Generate();
    }
}