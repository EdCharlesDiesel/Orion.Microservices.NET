using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
namespace Orion.API.TradingEconomics.Engine;

/// <summary>
/// Generates an alpha signal from normalized technical, sentiment, and macro indicators.
/// </summary>
public sealed class AlphaEngine : IAlphaEngine
{
    private const decimal LongThreshold = 0.25m;
    private const decimal ShortThreshold = -0.25m;
    private const decimal HighImpactConfidenceThreshold = 0.75m;

    /// <summary>
    /// Generates an alpha result for a currency pair.
    /// </summary>
    /// <param name="pair">The currency pair.</param>
    /// <param name="indicators">Normalized indicators used to calculate alpha.</param>
    /// <param name="macroEvents">Optional macro events used to suppress low-confidence signals.</param>
    /// <returns>An alpha result containing direction, score, confidence, and component scores.</returns>
    public AlphaResult Generate(string pair, List<NormalizedIndicator>? indicators, List<MacroEvent>? macroEvents = null)
    {
        if (string.IsNullOrWhiteSpace(pair))
            throw new ArgumentException("Pair is required.", nameof(pair));

        indicators ??= [];
        macroEvents ??= [];

        var trend = GetIndicatorValue(indicators, "TREND");
        var momentum = GetIndicatorValue(indicators, "MOMENTUM");
        var volatility = GetIndicatorValue(indicators, "VOLATILITY");
        var sentiment = GetIndicatorValue(indicators, "SENTIMENT");
        var macro = GetIndicatorValue(indicators, "MACRO");

        var volatilityPenalty = Math.Abs(volatility) * 0.10m;

        var alphaScore = Clamp(
            trend * 0.30m +
            momentum * 0.25m +
            sentiment * 0.20m +
            macro * 0.15m -
            volatilityPenalty,
            -1m,
            1m);

        var confidence = Math.Abs(alphaScore);
        var direction = GetDirection(alphaScore);

        var highImpactEvents = macroEvents.Count(x =>
            string.Equals(x.Impact, "HIGH", StringComparison.OrdinalIgnoreCase));

        if (highImpactEvents > 0 && confidence < HighImpactConfidenceThreshold)
            direction = "FLAT";

        return new AlphaResult
        {
            Pair = pair.Trim().ToUpperInvariant(),
            Direction = direction,
            AlphaScore = alphaScore,
            Confidence = confidence,
            TrendScore = trend,
            MomentumScore = momentum,
            VolatilityPenalty = volatilityPenalty,
            SentimentScore = sentiment,
            MacroScore = macro,
            HighImpactMacroEvents = highImpactEvents,
            TimestampUtc = DateTime.UtcNow
        };
    }

    private static string GetDirection(decimal alphaScore)
    {
        return alphaScore switch
        {
            >= LongThreshold => "LONG",
            <= ShortThreshold => "SHORT",
            _ => "FLAT"
        };
    }

    private static decimal GetIndicatorValue(List<NormalizedIndicator> indicators, string name)
    {
        var indicator = indicators.FirstOrDefault(x =>
            string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));

        return indicator?.Value ?? 0m;
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