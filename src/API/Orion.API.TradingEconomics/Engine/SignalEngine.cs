using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Generates directional FX trade signals from technical, regime, scenario and macro inputs.
    /// </summary>
    public sealed class SignalEngine : ISignalEngine
    {
        /// <inheritdoc />
        public SignalResult Generate(
            NormalizedMarketContext market,
            RegimeResult regime,
            ScenarioResult scenario,
            ProbabilisticScenarioResult probabilities,
            MacroSimulationResult macroSimulation)
        {
            ArgumentNullException.ThrowIfNull(market);
            ArgumentNullException.ThrowIfNull(regime);
            ArgumentNullException.ThrowIfNull(scenario);
            ArgumentNullException.ThrowIfNull(probabilities);
            ArgumentNullException.ThrowIfNull(macroSimulation);

            if (market.Candles == null || market.Candles.Count < 50)
                return SignalResult.NoTrade("Not enough candle data.");

            var technicalScore = CalculateTechnicalScore(market);
            var regimeScore = CalculateRegimeScore(regime);
            var scenarioScore = CalculateScenarioScore(scenario, probabilities);
            var macroScore = CalculateMacroScore(macroSimulation);

            var finalScore =
                technicalScore * 0.35m +
                regimeScore * 0.25m +
                scenarioScore * 0.25m +
                macroScore * 0.15m;

            finalScore = Clamp(finalScore);

            var confidence = Math.Abs(finalScore) * 100m;

            if (confidence < 55m)
            {
                return SignalResult.NoTrade(
                    $"Signal too weak. Score={finalScore:F2}, Confidence={confidence:F0}%");
            }

            var direction = finalScore > 0m ? "LONG" : "SHORT";

            return new SignalResult
            {
                Pair = market.Pair.Trim().ToUpperInvariant(),
                Direction = direction,
                Confidence = Math.Min(Math.Round(confidence, 2), 100m),
                Score = Math.Round(finalScore, 4),
                Reason =
                    $"Direction={direction}. " +
                    $"Technical={technicalScore:F2}, " +
                    $"Regime={regimeScore:F2}, " +
                    $"Scenario={scenarioScore:F2}, " +
                    $"Macro={macroScore:F2}, " +
                    $"Final={finalScore:F2}"
            };
        }

        private static decimal CalculateTechnicalScore(NormalizedMarketContext market)
        {
            var candles = market.Candles;
            var last = candles[^1];
            var previous = candles[^2];

            var close = last.Close;
            var previousClose = previous.Close;

            var sma20 = candles.TakeLast(20).Average(x => x.Close);
            var sma50 = candles.TakeLast(50).Average(x => x.Close);

            var score = 0m;

            score += close > sma20 ? 0.30m : -0.30m;
            score += sma20 > sma50 ? 0.30m : -0.30m;
            score += close > previousClose ? 0.20m : -0.20m;

            var candleRange = last.High - last.Low;
            var body = Math.Abs(last.Close - last.Open);

            if (candleRange > 0m && body / candleRange > 0.55m)
                score += close > last.Open ? 0.20m : -0.20m;

            return Clamp(score);
        }

        private static decimal CalculateRegimeScore(RegimeResult regime)
        {
            var regimeName = ResolveRegimeName(regime);

            return regimeName switch
            {
                "BULLISH" or "RISK_ON" or "RISKON" or "UPTREND" => 1.0m,
                "BEARISH" or "RISK_OFF" or "RISKOFF" or "DOWNTREND" => -1.0m,
                "RANGE" or "NEUTRAL" => 0.0m,
                _ => 0.0m
            };
        }

        private static decimal CalculateScenarioScore(
            ScenarioResult scenario,
            ProbabilisticScenarioResult probabilities)
        {
            var direction = scenario.Direction?.Trim().ToUpperInvariant() ?? string.Empty;

            var baseScore = direction switch
            {
                "LONG" or "BUY" or "BULLISH" => 1.0m,
                "SHORT" or "SELL" or "BEARISH" => -1.0m,
                _ => 0.0m
            };

            var probabilityWeight = Math.Clamp(probabilities.Probability, 0m, 100m) / 100m;

            return Clamp(baseScore * probabilityWeight);
        }

        private static decimal CalculateMacroScore(MacroSimulationResult macroSimulation)
        {
            var direction = macroSimulation.Direction?.Trim().ToUpperInvariant() ?? string.Empty;
            var confidence = Math.Clamp(macroSimulation.Confidence, 0m, 100m) / 100m;

            return Clamp(direction switch
            {
                "LONG" or "BUY" or "BULLISH" => confidence,
                "SHORT" or "SELL" or "BEARISH" => -confidence,
                _ => 0.0m
            });
        }

        private static string ResolveRegimeName(RegimeResult regime)
        {
            if (!string.IsNullOrWhiteSpace(regime.Name))
                return regime.Name.Trim().ToUpperInvariant();

            return regime.Regime.ToString().Trim().ToUpperInvariant();
        }

        private static decimal Clamp(decimal value)
        {
            return Math.Max(-1.0m, Math.Min(1.0m, value));
        }
    }
}