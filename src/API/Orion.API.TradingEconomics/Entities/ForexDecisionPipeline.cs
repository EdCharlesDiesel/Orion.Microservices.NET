using Orion.API.TradingEconomics.Engine;

namespace Orion.API.TradingEconomics.Entities
{
    public sealed class ForexDecisionPipeline
    {
        private readonly NormalizationEngine _normalization;
        private readonly NormalizedMarketContext _normalizedContext;
        private readonly RegimeEngine _regime;
        
        private readonly ScenarioEngine _scenario;
        private readonly ProbabilisticScenarioEngine _probabilisticScenario;
        private readonly DynamicMacroSimulationEngine _macroSimulation;
        private readonly SignalEngine _signal;
        private readonly RiskEngine _risk;
        private readonly PositionSizingEngine _positionSizing;
        private readonly FxPricingEngine _pricing;
        private readonly ExecutionEngine _execution;
        private readonly ExitEngine _exit;

        public ForexDecisionPipeline(
            NormalizationEngine normalization,
            NormalizedMarketContext normalizedContext,
            RegimeEngine regime,
            ScenarioEngine scenario,
            ProbabilisticScenarioEngine probabilisticScenario,
            DynamicMacroSimulationEngine macroSimulation,
            SignalEngine signal,
            RiskEngine risk,
            PositionSizingEngine positionSizing,
            FxPricingEngine pricing,
            ExecutionEngine execution,
            ExitEngine exit)
        {
            _normalizedContext = normalizedContext;
            _normalization = normalization;
            _regime = regime;
            _scenario = scenario;
            _probabilisticScenario = probabilisticScenario;
            _macroSimulation = macroSimulation;
            _signal = signal;
            _risk = risk;
            _positionSizing = positionSizing;
            _pricing = pricing;
            _execution = execution;
            _exit = exit;
        }

        public TradingDecision Run(ForexMarketInput input)
        {
            if (input == null)
                return TradingDecision.NoTrade("Market input is null.");

            var normalizedContext = new NormalizedMarketContext();
            var accountContext = new AccountContext();
            var orderBook = new OrderBook();
            
            var indicators = MacroMapper.ToIndicators(input.MacroEvents).ToList();

            var normalized = _normalization.Normalize(indicators);

            var regime = _regime.Detect(normalized.FirstOrDefault());

            var scenario = _scenario.Build(normalized.FirstOrDefault(), regime);

            var probabilities = _probabilisticScenario.Calculate(
                normalized.FirstOrDefault(),
                regime,
                scenario);

            var macroSimulation = _macroSimulation.Simulate(
                normalized.FirstOrDefault(),
                regime,
                probabilities);

            var signal = _signal.Generate(
                normalizedContext,
                regime,
                scenario,
                probabilities,
                macroSimulation);

            var risk = _risk.Evaluate(
                signal,
                normalizedContext,
                regime);

            if (!risk.IsAllowed)
                return TradingDecision.NoTrade(risk.Reason);

            var size = _positionSizing.Calculate(
                signal,
                risk,
                normalizedContext,
                accountContext);

            var price = _pricing.Price(
                signal.Pair,
                signal.Direction,
                size.PositionSize);

            var execution = _execution.Execute(
                orderBook,
                signal.Direction,
                size.PositionSize);

            var exit = _exit.Calculate(
                signal,
                execution,
                risk,
                normalized);

            return new TradingDecision
            {
                Pair = signal.Pair,
                Direction = signal.Direction,
                Confidence = signal.Confidence,
                Regime = regime.Name,
                Scenario = scenario.Name,
                RiskScore = risk.Score,
                PositionSize = size.PositionSize,
                EntryPrice = execution.ExecutedPrice,
                StopLoss = exit.StopLoss,
                TakeProfit = exit.TakeProfit,
                Reason = signal.Reason
            };
        }

        public static class MacroMapper
        {
            public static IEnumerable<EconomicIndicator> ToIndicators(
                IEnumerable<MacroEvent>? events)
            {
                if (events == null)
                    return Enumerable.Empty<EconomicIndicator>();

                return events.Select(e => new EconomicIndicator
                {
                    Country = e.Country,
                    Indicator = e.EventName,
                    Date = e.Date,
                    Value = e.Actual,
                    Forecast = e.Forecast,
                    Previous = e.Previous
                });
            }
        }
    }
}