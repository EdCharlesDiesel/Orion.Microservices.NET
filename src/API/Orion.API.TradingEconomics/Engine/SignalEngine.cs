using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class SignalEngine
    {
        public SignalResult Generate(
            NormalizedMarketContext market,
            RegimeResult regime,
            ScenarioResult scenario,
            ProbabilisticScenarioResult probabilities,
            MacroSimulationResult macroSimulation)
        {
            if (market.Candles.Count < 50)
            {
                return SignalResult.NoTrade("Not enough candle data.");
            }

            var technicalScore = CalculateTechnicalScore(market);
            var regimeScore = CalculateRegimeScore(regime);
            var scenarioScore = CalculateScenarioScore(scenario, probabilities);
            var macroScore = CalculateMacroScore(macroSimulation);

            var finalScore =
                technicalScore * 0.35m +
                regimeScore * 0.25m +
                scenarioScore * 0.25m +
                macroScore * 0.15m;

            var confidence = Math.Abs(finalScore) * 100m;

            if (confidence < 55m)
            {
                return SignalResult.NoTrade(
                    $"Signal too weak. Score={finalScore:F2}, Confidence={confidence:F0}%");
            }

            var direction = finalScore > 0 ? "LONG" : "SHORT";

            return new SignalResult
            {
                Pair = market.Pair,
                Direction = direction,
                Confidence = Math.Min(confidence, 100m),
                Score = finalScore,
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

            var close = (decimal)last.Close;
            var previousClose = (decimal)previous.Close;

            var sma20 = candles.TakeLast(20).Average(x => (decimal)x.Close);
            var sma50 = candles.TakeLast(50).Average(x => (decimal)x.Close);    
            decimal score = 0;

            if (close > sma20)
                score += 0.30m;
            else
                score -= 0.30m;

            if (sma20 > sma50)
                score += 0.30m;
            else
                score -= 0.30m;

            if (close > previousClose)
                score += 0.20m;
            else
                score -= 0.20m;

            var candleRange = (double)(last.High - last.Low);
            var body = Math.Abs((double)(last.Close - last.Open));

            if (candleRange > 0 && body / candleRange > 0.55)
            {
                score += close > last.Open ? 0.20m : -0.20m;
            }

            return Clamp(score);
        }

        private static decimal CalculateRegimeScore(RegimeResult regime)
        {
            return regime.Name.ToUpperInvariant() switch
            {
                "BULLISH" => 1.0m,
                "RISK_ON" => 0.8m,
                "UPTREND" => 0.8m,

                "BEARISH" => -1.0m,
                "RISK_OFF" => -0.8m,
                "DOWNTREND" => -0.8m,

                "RANGE" => 0.0m,
                "NEUTRAL" => 0.0m,
                _ => 0.0m
            };
        }
        private static decimal CalculateScenarioScore(ScenarioResult scenario,ProbabilisticScenarioResult probabilities)
        {
            var baseScore = scenario.Direction.ToUpperInvariant() switch
            {
                "LONG" => 1.0m,
                "BUY" => 1.0m,
                "BULLISH" => 1.0m,

                "SHORT" => -1.0m,
                "SELL" => -1.0m,
                "BEARISH" => -1.0m,

                _ => 0.0m
            };

            var probabilityWeight = probabilities.Probability / 100.0m;

            return Clamp(baseScore * probabilityWeight);
        }

        private static decimal CalculateMacroScore(MacroSimulationResult macroSimulation)
        {
            return Clamp(macroSimulation.Direction.ToUpperInvariant() switch
            {
                "LONG" => macroSimulation.Confidence / 100.0m,
                "BUY" => macroSimulation.Confidence / 100.0m,
                "BULLISH" => macroSimulation.Confidence / 100.0m,

                "SHORT" => -(macroSimulation.Confidence / 100.0m),
                "SELL" => -(macroSimulation.Confidence / 100.0m),
                "BEARISH" => -(macroSimulation.Confidence / 100.0m),

                _ => 0.0m
            });
        }

        private static decimal Clamp(decimal value)
        {
            return Math.Max(-1.0m, Math.Min(1.0m, value));
        }
    }
}
