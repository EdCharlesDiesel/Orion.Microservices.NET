using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Runs Monte Carlo simulations by randomly shuffling historical trades across
    /// N iterations and accumulating PnL against a fixed starting equity.
    /// </summary>
    public class MonteCarloEngine : IMonteCarloEngine
    {
        private const decimal InitialEquity = 100_000m;

        private readonly Random _random;

        /// <param name="random">
        /// Optional <see cref="Random"/> instance. When <c>null</c> a shared
        /// <see cref="Random.Shared"/> instance is used. Supply a seeded instance
        /// in tests for deterministic results.
        /// </param>
        public MonteCarloEngine(Random? random = null)
        {
            _random = random ?? Random.Shared;
        }

        /// <inheritdoc />
        public List<decimal> Run(List<TradeResult> trades, int simulations = 1000)
        {
            if (trades is null)
                throw new ArgumentNullException(nameof(trades));
            if (trades.Count == 0)
                throw new ArgumentException("Trade list must not be empty.", nameof(trades));
            if (simulations <= 0)
                throw new ArgumentOutOfRangeException(nameof(simulations), "Simulations must be greater than zero.");

            var results = new List<decimal>(simulations);

            for (int i = 0; i < simulations; i++)
            {
                var equity = InitialEquity;

                // Shuffle a copy so the original list is never mutated.
                var shuffled = trades.OrderBy(_ => _random.Next());

                foreach (var trade in shuffled)
                    equity += trade.PnL;

                results.Add(equity);
            }

            return results;
        }
    }
}