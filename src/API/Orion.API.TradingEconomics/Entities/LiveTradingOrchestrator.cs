// using Orion.API.TradingEconomics.Engine;
// using Orion.API.TradingEconomics.Engine.Interfaces;
//
// namespace Orion.API.TradingEconomics.Entities
// {
//     /// <summary>
//     /// Orchestrates full trading workflow from macro → execution.
//     /// </summary>
//     public sealed class LiveTradingOrchestrator : ILiveTradingOrchestrator
//     {
//         private readonly NormalizationEngine _normalization;
//         private readonly RegimeEngine _regime;
//         private readonly ScenarioEngine _scenario;
//         private readonly ProbabilisticScenarioEngine _probScenario;
//         private readonly DynamicMacroSimulationEngine _macro;
//         private readonly SignalEngine _signal;
//         private readonly RiskEngine _risk;
//         private readonly PositionSizingEngine _positionSizing;
//         private readonly FxPricingEngine _pricing;
//         private readonly ExecutionEngine _execution;
//         private readonly ExitEngine _exit;
//         private readonly TradeLifecycleEngine _tradeLifecycle;
//
//         public LiveTradingOrchestrator(
//             NormalizationEngine normalization,
//             RegimeEngine regime,
//             ScenarioEngine scenario,
//             ProbabilisticScenarioEngine probabilisticScenario,
//             DynamicMacroSimulationEngine macroSimulation,
//             SignalEngine signal,
//             RiskEngine risk,
//             PositionSizingEngine positionSizing,
//             FxPricingEngine pricing,
//             ExecutionEngine execution,
//             ExitEngine exit,
//             TradeLifecycleEngine tradeLifecycle)
//         {
//             _normalization = normalization ?? throw new ArgumentNullException(nameof(normalization));
//             _regime = regime ?? throw new ArgumentNullException(nameof(regime));
//             _scenario = scenario ?? throw new ArgumentNullException(nameof(scenario));
//             _probScenario = probabilisticScenario ?? throw new ArgumentNullException(nameof(probabilisticScenario));
//             _macro = macroSimulation ?? throw new ArgumentNullException(nameof(macroSimulation));
//             _signal = signal ?? throw new ArgumentNullException(nameof(signal));
//             _risk = risk ?? throw new ArgumentNullException(nameof(risk));
//             _positionSizing = positionSizing ?? throw new ArgumentNullException(nameof(positionSizing));
//             _pricing = pricing ?? throw new ArgumentNullException(nameof(pricing));
//             _execution = execution ?? throw new ArgumentNullException(nameof(execution));
//             _exit = exit ?? throw new ArgumentNullException(nameof(exit));
//             _tradeLifecycle = tradeLifecycle ?? throw new ArgumentNullException(nameof(tradeLifecycle));
//         }
//
//         /// <inheritdoc />
//         public LiveTradingResult Run(
//             ForexMarketInput input,
//             AccountContext account,
//             OrderBook orderBook)
//         {
//             ArgumentNullException.ThrowIfNull(input);
//             ArgumentNullException.ThrowIfNull(account);
//             ArgumentNullException.ThrowIfNull(orderBook);
//
//             // FIX: Normalize returns list → pick latest
//             var normalizedList = _normalization.Normalize(new List<ForexMarketInput>() _execution);
//
//             if (normalizedList.Count == 0)
//                 return LiveTradingResult.Blocked("NO_DATA", "No normalized data.");
//
//             var normalized = normalizedList.Last();
//
//             var regime = _regime.Detect(normalized);
//
//             var scenario = _scenario.Build(normalized, regime);
//
//             var probabilities = _probScenario.Calculate(
//                 normalized,
//                 regime,
//                 scenario);
//
//             var macro = _macro.Simulate(
//                 normalized,
//                 regime,
//                 probabilities);
//
//             var signal = _signal.Generate(
//                 new NormalizedMarketContext { Pair = input.Pair, Candles = input.Candles },
//                 regime,
//                 scenario,
//                 probabilities,
//                 macro);
//
//             if (signal.Direction == "NO_TRADE")
//                 return LiveTradingResult.Blocked("SIGNAL_BLOCKED", signal.Reason);
//
//             var risk = _risk.Evaluate(signal, normalized, regime);
//
//             if (!risk.IsAllowed)
//                 return LiveTradingResult.Blocked("RISK_BLOCKED", risk.Reason);
//
//             var size = _positionSizing.Calculate(
//                 signal,
//                 risk,
//                 new NormalizedMarketContext { Pair = input.Pair, Candles = input.Candles },
//                 account);
//
//             if (!size.IsAllowed)
//                 return LiveTradingResult.Blocked("POSITION_SIZE_BLOCKED", size.Reason);
//
//             var price = _pricing.Price(
//                 signal.Pair,
//                 signal.Direction,
//                 size.PositionSize);
//
//             var execution = _execution.Execute(
//                 orderBook,
//                 signal.Direction,
//                 size.PositionSize);
//
//             if (execution.FilledSize <= 0)
//                 return LiveTradingResult.Blocked("EXECUTION_FAILED", "Order was not filled.");
//
//             var exit = _exit.Calculate(
//                 signal,
//                 execution,
//                 risk,
//                 new List<NormalizedIndicator>
//                 {
//                     Capacity = 0,
//                     Pair = input.Pair,
//                     Candles = input.Candles
//                 });
//
//             var trade = _tradeLifecycle.CreatePlan(
//                 signal,
//                 risk,
//                 size,
//                 execution,
//                 exit);
//
//             return new LiveTradingResult
//             {
//                 Status = trade.Status,
//                 Pair = trade.Pair,
//                 Direction = trade.Direction,
//                 Confidence = signal.Confidence,
//                 Regime = regime.Regime.ToString(),
//                 Scenario = scenario.ScenarioName,
//                 PositionSize = trade.PositionSize,
//                 EntryPrice = trade.EntryPrice,
//                 StopLoss = trade.StopLoss,
//                 TakeProfit = trade.TakeProfit,
//                 RiskScore = risk.Score,
//                 Reason = trade.Reason,
//                 Trade = trade
//             };
//         }
//     }
// }