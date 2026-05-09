using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public class MonteCarloEngineTests
    {
        // ── helpers ────────────────────────────────────────────────────────────

        /// <summary>Returns a seeded engine for deterministic tests.</summary>
        private static MonteCarloEngine SeededEngine(int seed = 42) => new(new Random(seed));

        private static List<TradeResult> SampleTrades(int count = 5) =>
            Enumerable.Range(1, count)
                      .Select(i => new TradeResult { PnL = i * 100m })
                      .ToList();

        // ── guard clauses ──────────────────────────────────────────────────────

        [Fact]
        public void Run_NullTrades_ThrowsArgumentNullException()
        {
            var engine = SeededEngine();
            Assert.Throws<ArgumentNullException>(() => engine.Run(null!));
        }

        [Fact]
        public void Run_EmptyTrades_ThrowsArgumentException()
        {
            var engine = SeededEngine();
            Assert.Throws<ArgumentException>(() => engine.Run([]));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Run_InvalidSimulations_ThrowsArgumentOutOfRangeException(int simulations)
        {
            var engine = SeededEngine();
            Assert.Throws<ArgumentOutOfRangeException>(() => engine.Run(SampleTrades(), simulations));
        }

        // ── output shape ───────────────────────────────────────────────────────

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(1000)]
        public void Run_ReturnsExactlyNResults(int simulations)
        {
            var engine = SeededEngine();
            var results = engine.Run(SampleTrades(), simulations);
            Assert.Equal(simulations, results.Count);
        }

        // ── equity correctness ─────────────────────────────────────────────────

        [Fact]
        public void Run_SingleTrade_EveryEquityEqualsInitialPlusPnL()
        {
            // With one trade, every shuffle is the same — equity is always deterministic.
            var trade = new TradeResult { PnL = 500m };
            var engine = SeededEngine();

            var results = engine.Run([trade], simulations: 50);

            Assert.All(results, equity => Assert.Equal(100_500m, equity));
        }

        [Fact]
        public void Run_AllZeroPnL_AllResultsEqualInitialEquity()
        {
            var trades = Enumerable.Range(0, 10)
                                   .Select(_ => new TradeResult { PnL = 0m })
                                   .ToList();

            var results = SeededEngine().Run(trades, simulations: 200);

            Assert.All(results, equity => Assert.Equal(100_000m, equity));
        }

        [Fact]
        public void Run_KnownTrades_EachResultEqualsInitialPlusFixedSum()
        {
            // PnL sum is invariant under any permutation, so every result must be identical.
            var trades = SampleTrades();                      // PnL: 100+200+300+400+500 = 1500
            const decimal expectedEquity = 100_000m + 1_500m;

            var results = SeededEngine().Run(trades, simulations: 500);

            Assert.All(results, equity => Assert.Equal(expectedEquity, equity));
        }

        // ── immutability ───────────────────────────────────────────────────────

        [Fact]
        public void Run_DoesNotMutateInputList()
        {
            var trades = SampleTrades();
            var originalOrder = trades.Select(t => t.PnL).ToList();

            SeededEngine().Run(trades);

            Assert.Equal(originalOrder, trades.Select(t => t.PnL).ToList());
        }
    }
}