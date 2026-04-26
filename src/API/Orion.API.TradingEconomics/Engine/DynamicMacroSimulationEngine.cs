using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Engine for simulating dynamic macroeconomic state transitions.
    /// </summary>
    public class DynamicMacroSimulationEngine : IMacroSimulationEngine
    {
        private readonly IRegimeEngine _regimeEngine;
        private readonly ICorrelatedShockGenerator _shockGenerator;
        private readonly IMacroTransitionModel _transitionModel;

        /// <summary>
        /// Initializes a new instance of the DynamicMacroSimulationEngine.
        /// </summary>
        public DynamicMacroSimulationEngine()
        {
            _regimeEngine = new RegimeEngine();
            _shockGenerator = new CorrelatedShockGenerator();
            _transitionModel = new MacroTransitionModel();
        }

        /// <summary>
        /// Initializes a new instance with dependency injection.
        /// </summary>
        public DynamicMacroSimulationEngine(
            IRegimeEngine regimeEngine,
            ICorrelatedShockGenerator shockGenerator,
            IMacroTransitionModel transitionModel)
        {
            _regimeEngine = regimeEngine ?? throw new ArgumentNullException(nameof(regimeEngine));
            _shockGenerator = shockGenerator ?? throw new ArgumentNullException(nameof(shockGenerator));
            _transitionModel = transitionModel ?? throw new ArgumentNullException(nameof(transitionModel));
        }

        /// <inheritdoc />
        public List<MacroState> Run(MacroState initial, int steps)
        {
            if (initial == null)
                throw new ArgumentNullException(nameof(initial));

            if (steps <= 0)
                throw new ArgumentException("Steps must be greater than zero.", nameof(steps));

            var states = new List<MacroState> { initial };
            var currentRegime = MarketRegime.RiskOn;

            for (int step = 1; step <= steps; step++)
            {
                currentRegime = _regimeEngine.Next(currentRegime);
                var shocks = _shockGenerator.Generate();
                var nextState = _transitionModel.Next(states.Last(), shocks, currentRegime);
                states.Add(nextState);
            }

            return states;
        }

        /// <inheritdoc />
        public MacroSimulationResult Simulate(
            NormalizedIndicator normalized,
            RegimeResult regime,
            ProbabilisticScenarioResult probabilities)
        {
            if (normalized == null)
                throw new ArgumentNullException(nameof(normalized));

            if (regime == null)
                throw new ArgumentNullException(nameof(regime));

            if (probabilities == null)
                throw new ArgumentNullException(nameof(probabilities));

            var simulatedStates = new List<MacroState>();
            var currentRegime = regime.CurrentRegime;

            for (int i = 0; i < probabilities.ScenarioCount; i++)
            {
                currentRegime = _regimeEngine.Next(currentRegime);
                var shocks = _shockGenerator.GenerateWithProbabilities(probabilities);
                var nextState = _transitionModel.NextWithNormalization(normalized, shocks, currentRegime);
                simulatedStates.Add(nextState);
            }

            return new MacroSimulationResult
            {
                States = simulatedStates,
                FinalRegime = currentRegime,
                SuccessRate = CalculateSuccessRate(simulatedStates),
                TimestampUtc = DateTime.UtcNow
            };
        }

        private static double CalculateSuccessRate(List<MacroState> states)
        {
            if (states == null || states.Count == 0)
                return 0d;

            var successful = states.Count(s => s.IsStable);
            return Math.Round((double)successful / states.Count * 100, 2);
        }
    }

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

    /// <summary>
    /// Interface for regime engine.
    /// </summary>
    public interface IRegimeEngine
    {
        MarketRegime Next(MarketRegime current);
    }

    /// <summary>
    /// Interface for correlated shock generator.
    /// </summary>
    public interface ICorrelatedShockGenerator
    {
        ShockResult Generate();
        ShockResult GenerateWithProbabilities(ProbabilisticScenarioResult probabilities);
    }

    /// <summary>
    /// Placeholder implementations for dependencies.
    /// </summary>
    internal class RegimeEngine : IRegimeEngine
    {
        private static readonly Random Random = new();

        public MarketRegime Next(MarketRegime current)
        {
            var roll = Random.NextDouble();
            return roll < 0.3 ? SwitchRegime(current) : current;
        }

        private static MarketRegime SwitchRegime(MarketRegime current) =>
            current == MarketRegime.RiskOn ? MarketRegime.RiskOff : MarketRegime.RiskOn;
    }

    internal class CorrelatedShockGenerator : ICorrelatedShockGenerator
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

    internal class MacroTransitionModel : IMacroTransitionModel
    {
        public MacroState Next(MacroState current, ShockResult shocks, MarketRegime regime) => new()
        {
            GdpGrowth = current.GdpGrowth + shocks.GrowthShock,
            Inflation = current.Inflation + shocks.InflationShock,
            Sentiment = current.Sentiment + shocks.SentimentShock,
            IsStable = Math.Abs(shocks.GrowthShock) < 0.05m,
            TimestampUtc = DateTime.UtcNow
        };

        public MacroState NextWithNormalization(NormalizedIndicator normalized, ShockResult shocks, MarketRegime regime) => new()
        {
            GdpGrowth = normalized.GdpNormalized + shocks.GrowthShock,
            Inflation = normalized.InflationNormalized + shocks.InflationShock,
            Sentiment = normalized.SentimentNormalized + shocks.SentimentShock,
            IsStable = true,
            TimestampUtc = DateTime.UtcNow
        };
    }
}