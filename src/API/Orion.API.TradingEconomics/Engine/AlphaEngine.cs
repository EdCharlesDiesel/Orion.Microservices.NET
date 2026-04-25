using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class AlphaEngine
    {
        public AlphaResult Generate(
            string pair,
            List<NormalizedIndicator> indicators,
            List<MacroEvent>? macroEvents = null)
        {
            if (string.IsNullOrWhiteSpace(pair))
                throw new ArgumentException("Pair is required.", nameof(pair));

            indicators ??= new List<NormalizedIndicator>();
            macroEvents ??= new List<MacroEvent>();

            var trend = Get(indicators, "TREND");
            var momentum = Get(indicators, "MOMENTUM");
            var volatility = Get(indicators, "VOLATILITY");
            var sentiment = Get(indicators, "SENTIMENT");
            var macro = Get(indicators, "MACRO");

            var rawScore =
                trend * 0.30m +
                momentum * 0.25m +
                sentiment * 0.20m +
                macro * 0.15m -
                Math.Abs(volatility) * 0.10m;

            rawScore = Clamp(rawScore, -1m, 1m);

            var confidence = Math.Abs(rawScore);

            var direction = rawScore switch
            {
                >= 0.25m => "LONG",
                <= -0.25m => "SHORT",
                _ => "FLAT"
            };

            var highImpactEvents = macroEvents.Count(x =>
                string.Equals(x.Impact, "HIGH", StringComparison.OrdinalIgnoreCase));

            if (highImpactEvents > 0 && confidence < 0.75m)
                direction = "FLAT";

            return new AlphaResult
            {
                Pair = pair.Trim().ToUpperInvariant(),
                Direction = direction,
                AlphaScore = rawScore,
                Confidence = confidence,
                TrendScore = trend,
                MomentumScore = momentum,
                VolatilityPenalty = Math.Abs(volatility) * 0.10m,
                SentimentScore = sentiment,
                MacroScore = macro,
                HighImpactMacroEvents = highImpactEvents,
                TimestampUtc = DateTime.UtcNow
            };
        }

        private static decimal Get(List<NormalizedIndicator> indicators, string name)
        {
            var item = indicators.FirstOrDefault(x =>
                string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));

            return item?.Value ?? 0m;
        }

        private static decimal Clamp(decimal value, decimal min, decimal max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }
    }
}