using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;
using Orion.API.TradingEconomics.Interfaces;
using CorrelatedShockGenerator = Orion.API.TradingEconomics.Helpers.CorrelatedShockGenerator;

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
        /// Initializes a new instance with dependency injection.
        /// </summary>
        public DynamicMacroSimulationEngine(IRegimeEngine regimeEngine, ICorrelatedShockGenerator shockGenerator, IMacroTransitionModel transitionModel)
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
        public MacroSimulationResult Simulate(NormalizedIndicator normalized, RegimeResult regime, ProbabilisticScenarioResult probabilities)
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
}