using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public class DynamicMacroSimulationEngine
    {
        private readonly RegimeEngine _regime = new();
        private readonly CorrelatedShockGenerator _shock = new();
        private readonly MacroTransitionModel _transition = new();

        public List<MacroState> Run(MacroState initial, int steps)
        {
            var states = new List<MacroState> { initial };
            var regime = MarketRegime.RiskOn;

            for (int t = 1; t <= steps; t++)
            {
                regime = _regime.Next(regime);

                var shocks = _shock.Generate();

                var next = _transition.Next(states.Last(), shocks, regime);

                states.Add(next);
            }

            return states;
        }

        internal MacroSimulationResult Simulate(NormalizedIndicator normalized, RegimeResult regime, ProbabilisticScenarioResult probabilities)
        {
            throw new NotImplementedException();
        }
    }
}
